using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UTAMap.Models
{
    public class POISelection
    {
        public bool AcademicBuildings { get; set; }
        public bool AdministrativeBuildings { get; set; }
        public bool OffCampus { get; set; }
        public bool OnCampus { get; set; }
        public bool Playgrounds { get; set; }
        public bool ResidenceHalls { get; set; }
        public string SearchText { get; set; }
        public double Radius { get; set; }
    }
}