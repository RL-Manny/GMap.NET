using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Weather Infrared map provider (overlay)
    /// </summary>
    public class AzureWeatherInfraredMapProvider : AzureMapProviderBase
    {
        public static readonly AzureWeatherInfraredMapProvider Instance;

        static AzureWeatherInfraredMapProvider()
        {
            Instance = new AzureWeatherInfraredMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("B15C9D0E-F3E4-BF2A-AB8C-9D0E1F2A3B4C"); }
        }

        public override string Name
        {
            get { return "AzureMapsWeatherInfrared"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.weather.infrared.main"; }
        }

        AzureWeatherInfraredMapProvider()
        {
            MaxZoom = 15; // Weather infrared tiles are available up to zoom 15
        }
    }
}
