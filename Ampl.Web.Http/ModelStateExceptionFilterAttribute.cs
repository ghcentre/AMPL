using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Ampl.Web.Http
{
  public class ModelStateExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(HttpActionExecutedContext context)
    {
      if(context.Exception is ModelStateException)
      {
        var me = (context.Exception as ModelStateException);
        context.Response = context.Request.CreateErrorResponse((HttpStatusCode)422, me.ModelState);
      }
    }
  }
}
