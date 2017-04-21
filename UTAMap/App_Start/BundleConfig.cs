using System.Web;
using System.Web.Optimization;

namespace UTAMap
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/typeahead.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet").Include(
                      "~/Scripts/leaflet.js",
                      "~/Scripts/uta.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/leaflet.css"));
        }
    }
}
