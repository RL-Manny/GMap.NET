using System;

namespace GMap.NET.MapProviders.OpenStreetMap
{
    /// <summary>
    /// OpenStreetMap Hybrid provider - Satellite with roads overlay
    /// </summary>
    public class OpenStreetMapHybridProvider : OpenStreetMapProviderBase
    {
        public static readonly OpenStreetMapHybridProvider Instance;

        static OpenStreetMapHybridProvider()
        {
            Instance = new OpenStreetMapHybridProvider();
        }

        public string Version = "1";

        private OpenStreetMapHybridProvider()
        {
        }

        public override Guid Id
        {
            get { return new Guid("A9D8F2E3-C6B5-4A7E-9F8D-2E1C3B4A5F6E"); }
        }

        public override string Name
        {
            get { return "OpenStreetMapHybrid"; }
        }

        GMapProvider[] _overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[] { OpenStreetMapSatelliteProvider.Instance, this };
                }

                return _overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, string.Empty);
            return GetTileImageUsingHttp(url);
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            // Using Esri World Transportation service for roads overlay
            // Format: https://server.arcgisonline.com/ArcGIS/rest/services/Reference/World_Transportation/MapServer/tile/{z}/{y}/{x}
            return string.Format("https://server.arcgisonline.com/ArcGIS/rest/services/Reference/World_Transportation/MapServer/tile/{0}/{1}/{2}", zoom, pos.Y, pos.X);
        }
    }
}
