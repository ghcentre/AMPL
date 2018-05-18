using System.Web;
using System.Web.Optimization;
using Ampl.Web.Mvc;
using Ampl.Web.Optimization;

namespace Ampl.Web.Mvc.EditorTemplates.Web
{
  public class BundleConfig
  {
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        "~/Scripts/cldr.js",
        "~/Scripts/cldr/*.js",

        "~/Scripts/globalize.js",
        "~/Scripts/globalize/message.js",
        "~/Scripts/globalize/number.js",
        "~/Scripts/globalize/plural.js",
        "~/Scripts/globalize/date.js",
        "~/Scripts/globalize/currency.js",
        "~/Scripts/globalize/relative-time.js",
        "~/Scripts/globalize/*.js",

        "~/Scripts/ampl.globalize.fetch.js",

        "~/Scripts/jquery.validate.js",
        "~/Scripts/jquery.validate.globalize.js",
        "~/Scripts/jquery.validate.unobtrusive.js",

        "~/Scripts/ampl.bootstrap.jquery.validate.js",
        "~/Scripts/ampl.jquery.validate.unobtrusive.js"
      ).ForceOrder());

      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        "~/Scripts/bootstrap.js",
        "~/Scripts/respond.js"
      ).ForceOrder());

      bundles.Add(new StyleBundle("~/Content/css").Include(
        "~/Content/Fonts.css",
        "~/Content/bootstrap.css",
        "~/Content/BootstrapFixes.css",
        "~/Content/Site.css",
        "~/Content/Classes.css",
        "~/Content/Margins.css",
        "~/Content/Paddings.css"
      ).ForceOrder());
    }
  }
}
