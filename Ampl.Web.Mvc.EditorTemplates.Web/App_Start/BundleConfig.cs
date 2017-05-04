using System.Web;
using System.Web.Optimization;
using Ampl.Web.Mvc;
using Ampl.Web.Optimization;

namespace Ampl.Web.Mvc.EditorTemplates.Web
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/jquery-{version}.js").ForceOrder());

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.validate*",
                  "~/Scripts/ampl.bootstrap.jquery.validate.js").ForceOrder());

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*").ForceOrder());

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js").ForceOrder());

      bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Fonts.css",
                "~/Content/bootstrap.css",
                "~/Content/BootstrapFixes.css",
                "~/Content/site.css",
                "~/Content/Classes.css",
                "~/Content/Margins.css",
                "~/Content/Paddings.css"
                ).ForceOrder());
    }
  }
}
