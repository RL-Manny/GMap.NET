using System;

namespace GMap.NET.MapProviders.AMap
{
    /// <summary>
    ///     AMapSatelliteProvider provider
    /// </summary>
    public class AMapSatelliteProvider : AMapProviderBase
    {
        public static readonly AMapSatelliteProvider Instance;

        private static readonly string UrlFormat;

        AMapSatelliteProvider()
        {
        }

        static AMapSatelliteProvider()
        {
            UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=6&x={1}&y={2}&z={3}";
            Instance = new AMapSatelliteProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BC");

        public override string Name
        {
            get;
        } = "AMapSatellite";

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
