using System;
using System.Text.RegularExpressions;

namespace GMap.NET.MapProviders
{
    /// <summary>
    ///     BaiduHybridMapProvider provider
    /// </summary>
    public class BaiduHybridMapProvider : BaiduMapProviderBase
    {
        public static readonly BaiduHybridMapProvider Instance;

        private static readonly string UrlFormat;

        BaiduHybridMapProvider()
        {
        }

        static BaiduHybridMapProvider()
        {
            UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=sl&v={4}&udt=20140314";
            Instance = new BaiduHybridMapProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("AF522C29-9F94-4E9B-BDB3-346CD058AE7B");

        public override string Name
        {
            get;
        } = "BaiduHybridMap";

        GMapProvider[] _overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[] { BaiduSatelliteMapProvider.Instance, this };
                }
                return _overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);
            return GetTileImageUsingHttp(url);
        }

        #endregion

        protected override string GetVersion(string contentUsingHttp)
        {
            Regex regex = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"dem\":", RegexOptions.IgnoreCase);
            Match match = regex.Match(contentUsingHttp);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                if (groups.Count > 0)
                {
                    return groups[1].Value;
                }
            }

            return "039";
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            long num = pos.X - ((long)Math.Pow(2.0, zoom - 1));
            long num2 = ((long)Math.Pow(2.0, zoom - 1)) - pos.Y - 1;

            string str = num.ToString();
            string str2 = num2.ToString();

            if (str.StartsWith("-"))
            {
                str = "M" + str.Substring(1);
            }
            if (str2.StartsWith("-"))
            {
                str2 = "M" + str2.Substring(1);
            }

            return string.Format(UrlFormat, new object[] { GetServerNum(pos, MaxServer) + 1, str, str2, zoom, this.Version });
        }
    }
}
