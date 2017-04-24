using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class RouteSegment
    {
        public int seq { get; set; }
        public int node { get; set; }
        public int edge { get; set; }
        public double cost { get; set; }
        public string geom { get; set; }
    }
}