using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class Route
    {
        public List<POI> POIs { get; set; }
        public List<RouteSegment> Segments { get; set; }
        public double Cost
        {
            get
            {
                return Segments.Sum(s => s.cost);
            }
        }
    }
}