using Ampl.System;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Ampl.Web.Http
{
  public class ExposeExceptionMessageFilterAttribute : ExceptionFilterAttribute
  {
    private readonly Type _exceptionType;
    private readonly HttpStatusCode _statusCode;

    public ExposeExceptionMessageFilterAttribute(Type exceptionType, HttpStatusCode statusCode)
    {
      _exceptionType = Check.NotNull(exceptionType, nameof(exceptionType));
      _statusCode = statusCode;
    }

    public override void OnException(HttpActionExecutedContext context)
    {
      if(_exceptionType.IsAssignableFrom(context.Exception.GetType()))
      {
        context.Response = context.Request.CreateErrorResponse(_statusCode, context.Exception.Message);
      }
    }
  }
}
