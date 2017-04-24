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
            poi.AddRange(db.Database.SqlQuery<string>(@"SELECT name FROM buildings;").ToList());
            return Json(poi);
        }


        [HttpPost]
        public IHttpActionResult GetNearestPOI(POISelection data)
        {
            List<POI> nearestPOI = new List<POI>();

            string sourcePOIquery = $"SELECT name as name, st_asgeojson(geom) as geom, category as category FROM buildings As b " +
                                   $"WHERE UPPER(b.name) = UPPER('{data.SearchText}')";
            nearestPOI.AddRange(db.Database.SqlQuery<POI>(sourcePOIquery).ToList());

            if (data.Radius > 0)
            {
                if (data.AcademicBuildings || data.AdministrativeBuildings || data.OffCampus || data.OnCampus || data.Playgrounds || data.ResidenceHalls)
                {

                    if (data.AcademicBuildings)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                       $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Academic Building' " +
                                       $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";

                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }

                    if (data.AdministrativeBuildings)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                       $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Administrative Building' " +
                                       $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }

                    if (data.OffCampus)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                       $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Off Campus Apartments' " +
                                       $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }

                    if (data.OnCampus)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                       $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'On Campus Apartments' " +
                                       $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }

                    if (data.Playgrounds)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                       $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Playground' " +
                                       $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }

                    if (data.ResidenceHalls)
                    {
                        string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                      $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Residence Hall' " +
                                      $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                        nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                    }
                }
                else
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                        $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name " +
                                        $"and ST_DWithin(p1.geom::geography, p2.geom::geography, {data.Radius}*1609.344)";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }
            }

            else
            {
                if (data.AcademicBuildings)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                   $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Academic Building'" +
                                   $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                   $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }

                if (data.AdministrativeBuildings)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                   $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Administrative Building'" +
                                   $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                   $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }

                if (data.OffCampus)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                  $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Off Campus Apartments'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }

                if (data.OnCampus)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                  $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'On Campus Apartments'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }

                if (data.Playgrounds)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                  $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Playground'" +
                                  $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                  $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }

                if (data.ResidenceHalls)
                {
                    string query = $"SELECT p2.name as name, st_asgeojson(p2.geom) as geom, p2.category as category FROM buildings As p1, buildings As p2 " +
                                 $"WHERE UPPER(p1.name) = UPPER('{data.SearchText}') and p1.name <> p2.name and p2.category = 'Residence Hall'" +
                                 $"ORDER BY ST_Distance(p1.geom, p2.geom) " +
                                 $"LIMIT 1";
                    nearestPOI.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                }
            }
            return Json(nearestPOI);
        }


        [HttpPost]
        public IHttpActionResult GetPOIs(POISelection data)
        {
            List<POI> poi = new List<POI>();
            if (!String.IsNullOrEmpty(data.SearchText))
            {
                string query = $"SELECT name, st_asgeojson(geom) as geom, category FROM buildings As b " +
                                    $"WHERE UPPER(b.name) = UPPER('{data.SearchText}')";
                poi.AddRange(db.Database.SqlQuery<POI>(query).ToList());
                return Json(poi);
            }
            else
            {

                if (data.AcademicBuildings)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'Academic Building';").ToList());

                if (data.AdministrativeBuildings)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'Administrative Building';").ToList());

                if (data.OffCampus)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'Off Campus Apartments';").ToList());

                if (data.OnCampus)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'On Campus Apartments';").ToList());

                if (data.Playgrounds)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'Playground';").ToList());

                if (data.ResidenceHalls)
                    poi.AddRange(db.Database.SqlQuery<POI>(@"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b where b.category = 'Residence Hall';").ToList());

                return Json(poi);
            }

        }
    }
}
