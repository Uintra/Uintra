using System.Web.Optimization;

namespace Compent.uCommunity
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/quill.js",
                "~/Scripts/select2.js",
                "~/Scripts/dropzone.js",
                "~/Scripts/flatpickr/flatpickr.js",
                "~/Scripts/flatpickr/l10n/da.js",
                "~/App_Plugins/Core/Content/scripts/*.js",
                "~/App_Plugins/Core/Controls/FileUpload/file-upload.js",
                "~/App_Plugins/Core/Controls/LightboxGalery/LightboxGallery.js",
                "~/App_Plugins/Comments/Comment.js",
                "~/App_Plugins/News/Create/create-news.js",
                "~/App_Plugins/News/Edit/edit-news.js",
                "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/*.css",
                      "~/App_Plugins/Comments/_comments.css"));
        }
    }
}
