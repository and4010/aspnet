using System.Web;
using System.Web.Optimization;

namespace CHPOUTSRCMES.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquerysession.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
               "~/bower_components/jquery-ui/jquery-ui.min.js",
               "~/bower_components/jquery-ui/jquery-ui-combobox.js",
               "~/bower_components/jquery-ui/datepicker-zh-TW.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                "~/bower_components/slimScroll/jquery.slimscroll.min.js",
               "~/bower_components/fastclick/fastclick.js",
               "~/bower_components/adminlte/js/app.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquerydatatable").Include(
                        "~/bower_components/datatables/media/js/jquery.dataTables.min.js",
                        "~/bower_components/datatables/media/js/dataTables.bootstrap.min.js",
                        "~/bower_components/dataTables-select/js/dataTables.select.min.js",
                        "~/bower_components/datatables-button/js/dataTables.buttons.min.js",
                        "~/bower_components/datatables-button/js/buttons.bootstrap.min.js",
                        "~/bower_components/datatables-responsive/js/dataTables.responsive.min.js",
                        "~/bower_components/datatables-responsive/js/responsive.bootstrap.min.js",
                        "~/bower_components/datatables-fixedheader/js/dataTables.fixedHeader.min.js",
                        "~/bower_components/datatables-fixedheader/js/fixedHeader.bootstrap.min.js",
                        "~/bower_components/datatables-editor/js/dataTables.editor.min.js",
                        "~/bower_components/datatables-editor/js/editor.bootstrap.min.js",
                        "~/bower_components/datatables-button/js/buttons.print.min.js",
                        "~/bower_components/datatables-button/js/buttons.html5.min.js",
                        "~/bower_components/datatables-button/js/buttons.colVis.min.js",
                        "~/bower_components/jszip/dist/jszip.min.js",
                        "~/bower_components/pdfmake/build/pdfmake.min.js",
                        "~/bower_components/sweetalert2/sweetalert2.all.min.js",
                        "~/bower_components/sweetalert2/sweetalert2.min.js",
                        "~/bower_components/sweetalert2/promise.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      //"~/bower_components/metisMenu/dist/metisMenu.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      //"~/bower_components/metisMenu/dist/metisMenu.min.css",
                      "~/Content/timeline.css",
                      "~/Content/mes.web.css",
                      "~/bower_components/sweetalert2/sweetalert2.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                    "~/bower_components/datatables/media/css/dataTables.bootstrap.min.css",
                    "~/bower_components/editor/css/editor.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui-css").Include(
                "~/bower_components/jquery-ui/jquery-ui.min.css",
                "~/bower_components/jquery-ui/jquery-ui-combobox.css"));

            bundles.Add(new StyleBundle("~/Content/theme").Include(
                "~/bower_components/adminlte/css/AdminLTE.css",
                "~/bower_components/adminlte/css/skins/_all-skins.min.css"
                ));

        }
    }
}
