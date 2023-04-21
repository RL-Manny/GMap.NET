using System;

namespace GMap.NET.MapProviders.AMap
{
    /// <summary>
    ///     AMapProvider provider
    /// </summary>
    public class AMapProvider : AMapProviderBase
    {
        public static readonly AMapProvider Instance;

        private static readonly string UrlFormat;

        AMapProvider()
        {
        }

        static AMapProvider()
        {
            UrlFormat = "http://webrd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={1}&y={2}&z={3}";
            Instance = new AMapProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE88");

        public override string Name
        {
            get;
        } = "AMap";

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
