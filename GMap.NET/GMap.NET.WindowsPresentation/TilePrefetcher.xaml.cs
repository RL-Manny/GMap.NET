using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GMap.NET.MapProviders;
using static GMap.NET.WindowsPresentation.GMapTileCache;

namespace GMap.NET.WindowsPresentation
{
    /// <summary>
    ///     form helping to prefetch tiles on local db
    /// </summary>
    public partial class TilePrefetcher : Window
    {
        readonly GMapTileCache _tileCache;
        public bool ShowCompleteMessage = false;

        public TilePrefetcher()
        {
            _tileCache = new GMapTileCache(_done);

            InitializeComponent();

            GMaps.Instance.OnTileCacheComplete += OnTileCacheComplete;
            GMaps.Instance.OnTileCacheStart += OnTileCacheStart;
            GMaps.Instance.OnTileCacheProgress += OnTileCacheProgress;

            _tileCache.ProgressChanged += OnGMapTileCacheProgressChanged;
            _tileCache.Completed += OnGMapTileCacheCompleted;
        }

        readonly AutoResetEvent _done = new AutoResetEvent(true);

        void OnTileCacheComplete()
        {
            if (IsVisible)
            {
                _done.Set();

                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Label2.Text = "all tiles saved";
                    }));
            }
        }

        void OnTileCacheStart()
        {
            if (IsVisible)
            {
                _done.Reset();

                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Label2.Text = "saving tiles...";
                    }));
            }
        }

        void OnTileCacheProgress(int left)
        {
            if (IsVisible)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Label2.Text = left + " tile to save...";
                    }));
            }
        }

        public void Start(RectLatLng area, int zoom, GMapProvider provider, int sleep)
        {
            if (!_tileCache.IsBusy)
            {
                Label1.Text = "...";
                ProgressBar1.Value = 0;

                _tileCache.Start(area, zoom, provider, sleep);

                ShowDialog();
            }
        }

        public void Stop()
        {
            GMaps.Instance.OnTileCacheComplete -= OnTileCacheComplete;
            GMaps.Instance.OnTileCacheStart -= OnTileCacheStart;
            GMaps.Instance.OnTileCacheProgress -= OnTileCacheProgress;

            _tileCache.Stop();
        }

        private void OnGMapTileCacheCompleted(object sender, GMapTileCacheCompletedEventArgs e)
        {
            if (ShowCompleteMessage)
            {
                MessageBox.Show((!e.Cancelled ? "Prefetch Canceled!" : "Prefetch Canceled!") + "=> " + e.CurrentTileIndex.ToString() + " of " + e.TotalTileCount);
            }

            Close();
        }

        private void OnGMapTileCacheProgressChanged(object sender, GMapTileCacheProgressChangedEventArgs e)
        {
            Label1.Text = "Fetching tile at zoom (" + e.Zoom + "): " + e.CurrentTileIndex.ToString() + " of " +
                               e.TotalTileCount + ", complete: " + e.ProgressPercentage.ToString() + "%";
            ProgressBar1.Value = e.ProgressPercentage;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            Stop();

            base.OnClosed(e);
        }
    }
}
