using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Terrain map provider
    /// </summary>
    public class AzureTerrainMapProvider : AzureMapProviderBase
    {
        public static readonly AzureTerrainMapProvider Instance;

        static AzureTerrainMapProvider()
        {
            Instance = new AzureTerrainMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("8E2F6A7B-C0B1-8C9D-DE5F-6A7B8C9D0E1F"); }
        }

        public override string Name
        {
            get { return "AzureMapsTerrain"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.terra.main"; }
        }

        AzureTerrainMapProvider()
        {
            MaxZoom = 6; // Terra tiles are only available up to zoom 6
        }
    }
}
