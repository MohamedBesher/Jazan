using System.Web;
using System.Web.Optimization;

namespace Saned.Jazan.ControlPanel
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

           
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/plugins/jQuery/jQuery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                       "~/Scripts/jquery.unobtrusive-ajax.js"
                       
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap/js/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/bootstrap/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/dist/js/fileinput.min.js",
                //"~/Content/dist/js/jquery.nicescroll.js",
                "~/Content/dist/js/sweetalert-dev.js",
                "~/Content/plugins/datatables/jquery.dataTables.js",
                "~/Content/plugins/datatables/dataTables.bootstrap.js",
                "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                "~/Content/dist/js/app.min.js",
                "~/Content/dist/js/jquery.nicescroll.js",
                "~/Scripts/toastr.js ",
                "~/Scripts/app/services/NotificationService.js", 
                "~/Scripts/choose/chosen.jquery.min.js",
                //"~/Scripts/choose/ajax-chosen.js"
                "~/Scripts/choose/example/ajax-chosen.js",
                "~/Scripts/choose/example/chosen.ajaxaddition.jquery.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/choose").Include(
                "~/Scripts/choose/chosen.jquery.min.js"
                ));

           bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/css/bootstrap_ar.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/dist/css/ionicons.min.css",
                      "~/Content/dist/css/sweetalert.css",
                      "~/Content/dist/css/font-awesome.min.css",
                      "~/Content/dist/css/AdminLTE.css",
                      "~/Content/dist/css/skins/_all-skins.css",
                      "~/Content/plugins/datatables/dataTables.bootstrap.css",
                      "~/Content/bootstrap/datepicker.css",
                      "~/Content/dist/css/fileinput.min.css",
                      "~/Content/toastr.css",
                      "~/Scripts/choose/chosen.css"));


            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
               "~/Scripts/kendo/kendo.all.min.js",
               // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
               "~/Scripts/kendo/kendo.aspnetmvc.min.js"));



            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
               "~/Content/kendo/kendo.common-bootstrap.min.css",
               "~/Content/kendo/kendo.bootstrap.min.css"
               ));
            //in release Set to false
            BundleTable.EnableOptimizations = false;

            //in debugger Set to true
            //BundleTable.EnableOptimizations = true;

            //bundles.IgnoreList.Clear();
        }
    }
}
