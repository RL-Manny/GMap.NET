using System;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders.AMap
{
    public abstract class AMapProviderBase : GMapProvider
    {
        public AMapProviderBase()
        {
            MaxZoom = 18;
            MinZoom = 3;
            RefererUrl = "http://www.amap.com/";
            Copyright = string.Format("©{0} AutoNavi GS京", DateTime.Today.Year);    
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }
}
