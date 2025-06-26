using System;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Azure Dark Grey map provider
    /// </summary>
    public class AzureDarkMapProvider : AzureMapProviderBase
    {
        public static readonly AzureDarkMapProvider Instance;

        static AzureDarkMapProvider()
        {
            Instance = new AzureDarkMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("7D1E5F6A-BFA0-7B8C-CD4E-5F6A7B8C9D0E"); }
        }

        public override string Name
        {
            get { return "AzureMapsDark"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.base.darkgrey"; }
        }

        AzureDarkMapProvider()
        {
        }
    }
}
