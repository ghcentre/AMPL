using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Provides a way to return an 401 Unauthorized HTTP response to unauthenticated users
  /// and 403 Forbidden HTTP response to authenticated users.
  /// </summary>
  public class AccessDeniedResult : HttpStatusCodeResult
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessDeniedResult"/> class.
    /// </summary>
    public AccessDeniedResult()
      : base(HttpContext.Current.Request.IsAuthenticated ? 403 : 401)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessDeniedResult"/> class.
    /// </summary>
    /// <param name="request">The Web request.</param>
    public AccessDeniedResult(HttpRequestBase request)
      : base(request.IsAuthenticated ? 403 : 401)
    {
    }
  }
}