using System.Web;
using System.Web.Optimization;

namespace Chevron.HRPD.UI.MVC4
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                //"~/Scripts/jquery-{version}.js"));
                         "~/Scripts/jquery.min.js")); // the latest one v 1.9.1

            //bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            //  "~/Scripts/kendo/kendo.all.min.js",
            //    // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
            //  "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.ui.widget.js",
                        "~/Scripts/jquery.fileupload.js",
                        "~/Scripts/sweetalert-dev.js"
                //"~/Scripts/jquery.dialogextend.js"
                //"~/Scripts/ClientValidation.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery-migrate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/themes/base/jquery.fileupload.css",
                        "~/Content/sweetalert.css",
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                     "~/Content/kendo/kendo.common.min.css",
                     "~/Content/kendo/kendo.default.min.css"));

            bundles.IgnoreList.Clear();
        }
    }
}