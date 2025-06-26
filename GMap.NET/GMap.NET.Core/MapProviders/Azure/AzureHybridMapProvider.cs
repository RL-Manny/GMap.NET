using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Hybrid map provider (satellite with roads and labels)
    /// </summary>
    public class AzureHybridMapProvider : AzureMapProviderBase
    {
        public static readonly AzureHybridMapProvider Instance;

        static AzureHybridMapProvider()
        {
            Instance = new AzureHybridMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("6C0D4F5E-AE9F-6A7B-BC3D-4E5F6A7B8C9D"); }
        }

        public override string Name
        {
            get { return "AzureMapsHybrid"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.base.hybrid.road"; }
        }


        GMapProvider[] _overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (_overlays == null)
                {
                    _overlays = new GMapProvider[] { AzureSatelliteMapProvider.Instance, this };
                }

                return _overlays;
            }
        }

        AzureHybridMapProvider()
        {
        }
    }
}
