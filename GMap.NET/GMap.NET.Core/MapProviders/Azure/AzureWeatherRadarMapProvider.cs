using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Weather Radar map provider (overlay)
    /// </summary>
    public class AzureWeatherRadarMapProvider : AzureMapProviderBase
    {
        public static readonly AzureWeatherRadarMapProvider Instance;

        static AzureWeatherRadarMapProvider()
        {
            Instance = new AzureWeatherRadarMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("A04B8C9D-E2D3-AE1F-FA7B-8C9D0E1F2A3B"); }
        }

        public override string Name
        {
            get { return "AzureMapsWeatherRadar"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.weather.radar.main"; }
        }

        AzureWeatherRadarMapProvider()
        {
            MaxZoom = 15; // Weather radar tiles are available up to zoom 15
        }
    }
}
