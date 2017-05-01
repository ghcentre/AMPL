using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.EditorTemplates.Web
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
