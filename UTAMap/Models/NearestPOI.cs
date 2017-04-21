using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class NearestPOI
    {
        [Newtonsoft.Json.JsonRequired]
        public string geom { get; set; }

        public string sourcepoi { get; set; }
        public string name { get; set; }

        public string category { get; set; }
    }
}