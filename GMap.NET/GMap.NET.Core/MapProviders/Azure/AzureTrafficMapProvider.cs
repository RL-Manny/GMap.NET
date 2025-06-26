using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Traffic map provider (overlay)
    /// </summary>
    public class AzureTrafficMapProvider : AzureMapProviderBase
    {
        public static readonly AzureTrafficMapProvider Instance;

        static AzureTrafficMapProvider()
        {
            Instance = new AzureTrafficMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("9F3A7B8C-D1C2-9D0E-EF6A-7B8C9D0E1F2A"); }
        }

        public override string Name
        {
            get { return "AzureMapsTraffic"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.traffic.relative.main"; }
        }

        AzureTrafficMapProvider()
        {
        }
    }
}
