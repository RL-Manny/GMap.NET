using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Satellite map provider
    /// </summary>
    public class AzureSatelliteMapProvider : AzureMapProviderBase
    {
        public static readonly AzureSatelliteMapProvider Instance;

        static AzureSatelliteMapProvider()
        {
            Instance = new AzureSatelliteMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("5B9C3F4E-9D8E-5F6A-AB2C-3D4E5F6A7B8C"); }
        }

        public override string Name
        {
            get { return "AzureMapsSatellite"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.imagery"; }
        }

        AzureSatelliteMapProvider()
        {
            MaxZoom = 19; // Imagery is available up to zoom 19
        }
    }
}
