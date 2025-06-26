using System;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// Base class for Azure Map providers
    /// </summary>
    public abstract class AzureMapProviderBase : GMapProvider
    {
        private string _subscriptionKey;
        public string SubscriptionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_subscriptionKey))
                {
                    _subscriptionKey = GMapProviders.AzureMap.SubscriptionKey;
                }

                return _subscriptionKey;
            }
            set { _subscriptionKey = value; }
        }

        /// <summary>
        /// Azure Maps API version
        /// </summary>
        public string ApiVersion { get; set; } = "2024-04-01";

        /// <summary>
        /// Azure Maps base URL
        /// </summary>
        protected const string BaseUrl = "https://atlas.microsoft.com";

        /// <summary>
        /// Language for map labels (IETF language tag)
        /// </summary>
        public string Language { get; set; } = "en-US";

        /// <summary>
        /// View parameter for geopolitically disputed regions
        /// </summary>
        public string View { get; set; } = "Unified";

        /// <summary>
        /// Tile size (256 or 512 pixels)
        /// </summary>
        public int TileSize { get; set; } = 256;

        public AzureMapProviderBase()
        {
            MaxZoom = 22;
            RefererUrl = "https://atlas.microsoft.com/";
            Copyright = "© Microsoft Corporation";
        }

        public override Guid Id
        {
            get
            {
                throw new NotImplementedException("Must be implemented in derived class");
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException("Must be implemented in derived class");
            }
        }

        /// <summary>
        ///     timeout for provider connections
        /// </summary>
        public static int Timeout = 5 * 1000;

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
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

        /// <summary>
        /// Gets the tileset ID for the specific provider
        /// </summary>
        protected abstract string TilesetId { get; }

        /// <summary>
        /// Builds the tile URL for Azure Maps
        /// </summary>
        /// <param name="pos">Tile position</param>
        /// <param name="zoom">Zoom level</param>
        /// <returns>Complete tile URL</returns>
        protected string BuildTileUrl(GPoint pos, int zoom)
        {
            var url = $"{BaseUrl}/map/tile?api-version={ApiVersion}&tilesetId={TilesetId}&zoom={zoom}&x={pos.X}&y={pos.Y}";

            // Add optional parameters
            if (TileSize != 256)
            {
                url += $"&tileSize={TileSize}";
            }

            if (!string.IsNullOrEmpty(Language) && Language != "en-US")
            {
                url += $"&language={Language}";
            }

            if (!string.IsNullOrEmpty(View) && View != "Unified")
            {
                url += $"&view={View}";
            }

            if (!string.IsNullOrEmpty(SubscriptionKey))
            {
                url += $"&subscription-key={SubscriptionKey}";
            }

            return url;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = BuildTileUrl(pos, zoom);
            return GetTileImageUsingHttp(url);
        }
    }

    /// <summary>
    /// Azure Map Road provider
    /// </summary>
    public class AzureMapProvider : AzureMapProviderBase
    {
        public static readonly AzureMapProvider Instance;

        static AzureMapProvider()
        {
            Instance = new AzureMapProvider();
        }

        public override Guid Id
        {
            get { return new Guid("4A8B2F3E-8C7D-4E5F-9A1B-2C3D4E5F6A7B"); }
        }

        public override string Name
        {
            get { return "AzureMaps"; }
        }

        protected override string TilesetId
        {
            get { return "microsoft.base.road"; }
        }

        AzureMapProvider()
        {
        }
    }
}
