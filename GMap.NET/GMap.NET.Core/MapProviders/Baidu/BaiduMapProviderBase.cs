using System;
using GMap.NET.Internals;

namespace GMap.NET.MapProviders
{
    public abstract class BaiduMapProviderBase : GMapProvider
    {
        public static readonly int MaxServer;

        private string _version;

        static BaiduMapProviderBase()
        {
            MaxServer = 9;
        }

        public BaiduMapProviderBase()
        {
            MinZoom = 3;
            MaxZoom = 19;
            RefererUrl = string.Format("http://q{0}.baidu.com/", MaxServer.ToString());
            Copyright = string.Format("\x00a9 Baidu! Inc. - Map data & Imagery \x00a9{0} NAVTEQ", DateTime.Today.Year);
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual string CnName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return BaiduProjectionJS.Instance;
            }
        }

        GMapProvider[] _overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[] { this };
                }

                return _overlays;
            }
        }

        public override void OnInitialized()
        {
            {
                string url = "http://map.baidu.com";
                try
                {
                    string contentUsingHttp = Cache.Instance.GetContent(url, CacheType.UrlCache, TimeSpan.FromHours(24.0));
                    if (string.IsNullOrEmpty(contentUsingHttp))
                    {
                        contentUsingHttp = base.GetContentUsingHttp(url);
                        if (!string.IsNullOrEmpty(contentUsingHttp))
                        {
                            Cache.Instance.SaveContent(url, CacheType.UrlCache, contentUsingHttp);
                        }
                    }
                    if (!string.IsNullOrEmpty(contentUsingHttp))
                    {
                        _version = GetVersion(contentUsingHttp);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected virtual string GetVersion(string contentUsingHttp)
        {
            throw new NotImplementedException();
        }
    }
}
