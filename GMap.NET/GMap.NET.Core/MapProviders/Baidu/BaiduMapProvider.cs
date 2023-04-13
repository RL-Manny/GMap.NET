using System;
using System.Text.RegularExpressions;

namespace GMap.NET.MapProviders
{
    /// <summary>
    ///     BaiduMapProvider provider
    /// </summary>
    public class BaiduMapProvider : BaiduMapProviderBase
    {
        public static readonly BaiduMapProvider Instance;

        private static readonly string UrlFormat;

        BaiduMapProvider()
        {
        }

        static BaiduMapProvider()
        {
            UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20150213";
            Instance = new BaiduMapProvider();
        }

        #region GMapProvider Members

        public override Guid Id
        {
            get;
        } = new Guid("5532ECC6-6561-4451-BF2D-22E86D0DC9F8");

        public override string Name
        {
            get;
        } = "BaiduMap";

        public override string CnName
        {
            get;
        } = "百度普通地图";

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

        protected override string GetVersion(string contentUsingHttp)
        {
            Regex regex = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"satellite\":", RegexOptions.IgnoreCase);
            Match match = regex.Match(contentUsingHttp);
            if (match.Success)
            {
                GroupCollection groups3 = match.Groups;
                if (groups3.Count > 0)
                {
                    return groups3[1].Value;
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

            int serverNum = GetServerNum(pos, MaxServer) + 1;

            return string.Format(UrlFormat, new object[] { serverNum, str, str2, zoom });
        }
    }
}
