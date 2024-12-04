using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using GMap.NET.Internals;
using GMap.NET.MapProviders;

namespace GMap.NET.WindowsPresentation
{
    public class GMapTileCache
    {
        readonly BackgroundWorker _worker = new BackgroundWorker();
        List<GPoint> _list = new List<GPoint>();
        int _zoom;
        GMapProvider _provider;
        int _sleep;
        int _all;
        RectLatLng _area;
        GSize _maxOfTiles;

        readonly AutoResetEvent _done;

        volatile bool _stopped;

        public GMapTileCache(AutoResetEvent done = null)
        {
            _done = done;

            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        public bool IsBusy => _worker.IsBusy;

        public event EventHandler Started;
        public event EventHandler<GMapTileCacheProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<GMapTileCacheCompletedEventArgs> Completed;

        public void Start(RectLatLng area, int zoom, GMapProvider provider, int sleep)
        {
            if (!IsBusy)
            {
                _area = area;
                _zoom = zoom;
                _provider = provider;
                _sleep = sleep;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;
                GMaps.Instance.BoostCacheEngine = true;

                _worker.RunWorkerAsync();

                Started?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            _done?.Set();

            if (IsBusy)
            {
                _worker.CancelAsync();
            }

            GMaps.Instance.CancelTileCaching();

            _stopped = true;

            _done?.Close();

            Completed?.Invoke(this, new GMapTileCacheCompletedEventArgs(_zoom, _provider, 0, _all, true));
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _list.Clear();

            GMaps.Instance.UseMemoryCache = true;
            GMaps.Instance.CacheOnIdleRead = true;
            GMaps.Instance.BoostCacheEngine = false;

            Completed?.Invoke(this, new GMapTileCacheCompletedEventArgs(_zoom, _provider, (int)e.Result, _all, false));
        }

        bool CacheTiles(int zoom, GPoint p)
        {
            foreach (var type in _provider.Overlays)
            {
                Exception ex;
                PureImage img;

                // tile number inversion(BottomLeft -> TopLeft) for pergo maps
                if (type is TurkeyMapProvider)
                {
                    img = GMaps.Instance.GetImageFrom(type, new GPoint(p.X, _maxOfTiles.Height - p.Y), zoom, out ex);
                }
                else // ok
                {
                    img = GMaps.Instance.GetImageFrom(type, p, zoom, out ex);
                }

                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_list != null)
            {
                _list.Clear();
                _list = null;
            }

            _list = _provider.Projection.GetAreaTileList(_area, _zoom, 0);
            _maxOfTiles = _provider.Projection.GetTileMatrixMaxXY(_zoom);
            _all = _list.Count;

            int countOk = 0;
            int retry = 0;

            Stuff.Shuffle(_list);

            for (int i = 0; i < _all; i++)
            {
                if (_worker.CancellationPending)
                    break;

                var p = _list[i];
                {
                    if (CacheTiles(_zoom, p))
                    {
                        countOk++;
                        retry = 0;
                    }
                    else
                    {
                        if (++retry <= 1) // retry only one
                        {
                            i--;
                            Thread.Sleep(1111);
                            continue;
                        }
                        else
                        {
                            retry = 0;
                        }
                    }
                }

                _worker.ReportProgress((i + 1) * 100 / _all, i + 1);

                Thread.Sleep(_sleep);
            }

            e.Result = countOk;

            if (!_stopped)
            {
                _done?.WaitOne();
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(this, new GMapTileCacheProgressChangedEventArgs(_zoom, _provider, (int)e.UserState, _all, e.ProgressPercentage));
        }

        public class GMapTileCacheProgressChangedEventArgs : EventArgs
        {
            public readonly int Zoom;
            public readonly GMapProvider Provider;
            public readonly int CurrentTileIndex;
            public readonly int TotalTileCount;
            public readonly double ProgressPercentage;

            public GMapTileCacheProgressChangedEventArgs(int zoom, GMapProvider provider, int currentTileIndex, int totalTileCount, double progressPercentage)
            {
                Zoom = zoom;
                Provider = provider;
                CurrentTileIndex = currentTileIndex;
                TotalTileCount = totalTileCount;
                ProgressPercentage = progressPercentage;
            }
        }

        public class GMapTileCacheCompletedEventArgs : EventArgs
        {
            public readonly int Zoom;
            public readonly GMapProvider Provider;
            public readonly int CurrentTileIndex;
            public readonly int TotalTileCount;
            public readonly bool Cancelled;

            public GMapTileCacheCompletedEventArgs(int zoom, GMapProvider provider, int currentTileIndex, int totalTileCount, bool cancelled)
            {
                Zoom = zoom;
                Provider = provider;
                CurrentTileIndex = currentTileIndex;
                TotalTileCount = totalTileCount;
                Cancelled = cancelled;
            }
        }
    }
}
