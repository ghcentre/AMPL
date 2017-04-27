using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ampl.System;
using Ampl.Web.Mvc.ViewModels;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Provides a set of <see langword="static" /> methods to extend the functionality of
  /// the <see cref="ControllerBase"/> class to show Alert messages.
  /// </summary>
  public static class ControllerBaseAlertExtensions
  {
    /// <summary>
    /// Adds the alert to be displayed when the layout page is rendered.
    /// </summary>
    /// <param name="controllerBase"><see cref="ControllerBase"/></param>
    /// <param name="text">A text to be displayed</param>
    /// <param name="alertContextualClass">The alert contextual class. Default is Success.</param>
    /// <param name="heading">The alert heading. Default is no heading.</param>
    /// <param name="dismissible"><see langword="true" /> to make alert dismissible. The default
    /// is <see langword="false" /></param>
    public static void AddAlert(
      this ControllerBase controllerBase,
      string text,
      AlertContextualClass alertContextualClass = AlertContextualClass.Success,
      string heading = null,
      bool dismissible = false)
    {
      Check.NotNull(controllerBase, nameof(controllerBase));

      var list = (controllerBase.TempData[AlertConfiguration.AlertTempDataKey] as List<AlertViewModel>);
      if(list == null)
      {
        controllerBase.TempData[AlertConfiguration.AlertTempDataKey] = list = new List<AlertViewModel>();
      }
      list.Add(new AlertViewModel() {
        ContextalClass = alertContextualClass,
        Dismissible = dismissible,
        Heading = heading.With(x => new HtmlString(x)),
        Text = text.With(x => new HtmlString(x)),
      });
    }
  }

  /// <summary>
  /// Alrert message configuration
  /// </summary>
  internal static class AlertConfiguration
  {
    /// <summary>
    /// Name of the TempData key containing the list of Alert View Models.
    /// </summary>
    public const string AlertTempDataKey = "AMPL_Web_Mvc_Alert_Temp_Data";
  }
}
