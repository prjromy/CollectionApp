using System.Web;
using System.Web.Optimization;

namespace GarbageCollection
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 //"~/Content/font-awesome.css",

                "~/admin-lte/dist/css/adminlte.css",
                     // "~/Content/bootstrap.css",
                      "~/Content/LoaderSite.css",
                      "~/Content/Site.css",
                      
                      "~/admin-lte/css/skins/skin-blue.css"
                      ));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                       "~/Content/messagebox.css ",
          "~/Content/ch-dialog.css",
                "~/admin-lte/dist/css/adminlte.min.css",
                "~/admin-lte/dist/css/LoaderAdminLTE.css",
              //  "~/Content/font-awesome.min.css",
                "~/admin-lte/dist/css/skins/_all-skins.min.csss",
                "~/admin-lte/plugins/iCheck/flat/blue.css",
                "~/Content/bootstrap.css"
              //"~/bootstrap/css/bootstrap-theme.min.css"
                ));


        //bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
        //    // "~/admin-lte/js/app.js",
        //     //"~/admin-lte/plugins/jquery/jquery.min.js",
        //     "~/admin-lte/dist/js/adminlte.min.js",
        //    // "~/admin-lte/plugins/fastclick/fastclick.js",
        //     "~/Scripts/jquery.form.min.js",
         
        //     "~/Scripts/ch-treeview.js"

        //     ));
            BundleTable.EnableOptimizations = true;

        }

    }
}
