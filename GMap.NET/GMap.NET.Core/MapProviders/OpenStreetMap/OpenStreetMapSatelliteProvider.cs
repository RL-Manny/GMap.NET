using System;

namespace GMap.NET.MapProviders.OpenStreetMap
{
    /// <summary>
    /// OpenStreetMap Satellite provider using Esri World Imagery
    /// </summary>
    public class OpenStreetMapSatelliteProvider : OpenStreetMapProviderBase
    {
        public static readonly OpenStreetMapSatelliteProvider Instance;

        static OpenStreetMapSatelliteProvider()
        {
            Instance = new OpenStreetMapSatelliteProvider();
        }

        public string Version = "1";

        private OpenStreetMapSatelliteProvider()
        {
        }

        public override Guid Id
        {
            get { return new Guid("B8C5C554-C7B7-44B9-9F23-4EC8F5C6C5F7"); }
        }

        public override string Name
        {
            get { return "OpenStreetMapSatellite"; }
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

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, string.Empty);
            return GetTileImageUsingHttp(url);
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            // Using Esri World Imagery service
            // Format: https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}
            return string.Format("https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{0}/{1}/{2}", zoom, pos.Y, pos.X);
        }
    }
}
