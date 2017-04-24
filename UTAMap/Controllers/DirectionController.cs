using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UTAMap.Models;

namespace UTAMap.Controllers
{
    public class DirectionController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpPost]
        public IHttpActionResult GetDirections(SearchDirection direction)
        {            
            string query = $"SELECT id FROM ways_vertices_pgr " +
                $"ORDER BY ST_Distance(st_setsrid(st_makepoint(lon, lat), 4326)::geography, ST_Centroid((SELECT geom FROM buildings b WHERE UPPER(b.name) LIKE UPPER('{direction.From}')))::geography) LIMIT 1";
            List<String> sourceRoads = db.Database.SqlQuery<String>(query).ToList();
            query = $"SELECT id FROM ways_vertices_pgr " +
                $"ORDER BY ST_Distance(st_setsrid(st_makepoint(lon, lat), 4326)::geography, ST_Centroid((SELECT geom FROM buildings b WHERE UPPER(b.name) LIKE UPPER('{direction.To}')))::geography) LIMIT 1";
            List<String> targetRoads = db.Database.SqlQuery<String>(query).ToList();
            List<Route> routes = new List<Route>();
            foreach (var source in sourceRoads)
            {
                foreach (var target in targetRoads)
                {
                    routes.Add(new Route()
                    {
                        Segments = GetShortestPath(source, target)
                    });
                }
            }
            Route shortestRoute = routes.OrderBy(r => r.Cost).FirstOrDefault();
            shortestRoute.POIs = new List<POI>();
            query = $"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b WHERE UPPER(b.name) LIKE UPPER('{direction.From}')";
            shortestRoute.POIs.Add(db.Database.SqlQuery<POI>(query).FirstOrDefault());
            query = $"SELECT id, name, ST_AsGeoJSON(geom) as geom, category FROM buildings b WHERE UPPER(b.name) LIKE UPPER('{direction.To}')";
            shortestRoute.POIs.Add(db.Database.SqlQuery<POI>(query).FirstOrDefault());
            return Json(shortestRoute);
        }

        private List<RouteSegment> GetShortestPath(string source, string target)
        {
            string query = "SELECT seq, node, edge, di.cost AS cost, ST_AsGeoJSON(the_geom) AS geom " +
                $"FROM pgr_dijkstra('SELECT gid as id, source, target, cost FROM ways', {source}, {target}, false) as di " +
                "JOIN ways pt ON di.edge = pt.gid";

            List<RouteSegment> routes = db.Database.SqlQuery<RouteSegment>(query).ToList();
            return routes;
        }
    }
}
