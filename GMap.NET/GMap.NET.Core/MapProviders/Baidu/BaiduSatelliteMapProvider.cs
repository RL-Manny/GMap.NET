using System;
using System.Text.RegularExpressions;

namespace GMap.NET.MapProviders
{
    /// <summary>
    ///     BaiduSatelliteMapProvider provider
    /// </summary>
    public class BaiduSatelliteMapProvider : BaiduMapProviderBase
    {
        public static readonly BaiduSatelliteMapProvider Instance;

        private static readonly string UrlFormat;

        private readonly string _fm;
        private readonly string _type;

        BaiduSatelliteMapProvider()
        {
            _type = "sate";
            _fm = "46";
        }

        static BaiduSatelliteMapProvider()
        {
            UrlFormat = "http://shangetu{0}.map.bdimg.com/it/u=x={1};y={2};z={3};v={4};type={5}&fm={6}&udt=20140929";
            Instance = new BaiduSatelliteMapProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("EA7925B1-29A0-45A8-BD3C-5CC96C2ACCA4");

        public override string Name
        {
            get;
        } = "BaiduSatelliteMap";

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);
            return GetTileImageUsingHttp(url);
        }

        #endregion

        protected override string GetVersion(string contentUsingHttp)
        {
            Regex regex = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"normalTraffic\":", RegexOptions.IgnoreCase);
            Match match = regex.Match(contentUsingHttp);
            if (match.Success)
            {
                GroupCollection groups2 = match.Groups;
                if (groups2.Count > 0)
                {
                    return groups2[1].Value;
                }
            }

            return "009";
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

            return string.Format(UrlFormat, new object[] { GetServerNum(pos, MaxServer) + 1, str, str2, zoom, Version, _type, _fm });
        }
    }
}
