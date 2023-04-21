using System;

namespace GMap.NET.MapProviders.AMap
{
    /// <summary>
    ///     AMapHybridProvider provider
    /// </summary>
    public class AMapHybridProvider : AMapProviderBase
    {
        public static readonly AMapHybridProvider Instance;

        private static readonly string UrlFormat;

        AMapHybridProvider()
        {
        }

        static AMapHybridProvider()
        {
            UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=8&x={1}&y={2}&z={3}";
            Instance = new AMapHybridProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE87");

        public override string Name
        {
            get;
        } = "AMapHybrid";

        GMapProvider[] _overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[] { AMapSatelliteProvider.Instance, this };
                }
                return _overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            try
            {
                return GetTileImageUsingHttp(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            long num = ((pos.X + pos.Y) % 4) + 1;
            return string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
        }
    }
}
