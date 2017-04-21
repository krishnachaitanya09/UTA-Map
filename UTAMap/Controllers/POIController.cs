using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UTAMap.Models;

namespace UTAMap.Controllers
{
    public class POIController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        public IHttpActionResult GetAllPOIs()
        {
            List<string> poi = new List<string>();
            poi.AddRange(db.Database.SqlQuery<string>(@"Select name from ""Buildings"";").ToList());
            return Json(poi);
        }


        [HttpPost]
        public IHttpActionResult GetPOIs(POISelection data)
        {

            if (!String.IsNullOrEmpty(data.SearchText))
            {
                List<NearestPOI> nearestPOI = new List<NearestPOI>();

                if (data.AcademicBuildings)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                   $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'Academic Building'" +
                                   $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                   $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                if (data.AdministrativeBuildings)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                   $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'Administrative Building'" +
                                   $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                   $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                if (data.OffCampus)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                  $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'Off Campus Apartments'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                if (data.OnCampus)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                  $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'On Campus Apartments'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                if (data.Playgrounds)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                  $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'Playground'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                if (data.ResidenceHalls)
                {
                    string query = $"SELECT p1.name as sourcePOI, p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM \"Buildings\" As p1, \"Buildings\" As p2 " +
                                 $"WHERE p1.name = '{data.SearchText}' and p1.name<> p2.name and p2.category = 'Residence Hall'" +
                                 $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                 $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<NearestPOI>(query).ToList());
                }

                return Json(nearestPOI);
            }
            else
            {
                List<POI> poi = new List<POI>();
                if (data.AcademicBuildings)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'Academic Building';").ToList());

                if (data.AdministrativeBuildings)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'Administrative Building';").ToList());

                if (data.OffCampus)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'Off Campus Apartments';").ToList());

                if (data.OnCampus)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'On Campus Apartments';").ToList());

                if (data.Playgrounds)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'Playground';").ToList());

                if (data.ResidenceHalls)
                    poi.AddRange(db.Buildings.SqlQuery(@"select id, name, st_asgeojson(geom) as geom, category from ""Buildings"" b where b.category = 'Residence Hall';").ToList());

                return Json(poi);
            }

        }
    }
}
