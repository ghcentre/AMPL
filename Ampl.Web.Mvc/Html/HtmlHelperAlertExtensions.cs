using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ampl.Web.Mvc.ViewModels;

namespace Ampl.Web.Mvc.Html
{
    /// <summary>
    /// Provides <see langword="static"/> methods for Alert View Model retrieval.
    /// </summary>
    public static class HtmlHelperAlertExtensions
    {
        /// <summary>
        /// Retrieves the Alert View Models set by the
        /// <see cref="ControllerBaseAlertExtensions.AddAlert(ControllerBase, string, AlertContextualClass, string, bool)"/>
        /// </summary>
        /// <typeparam name="TModel">The Model Type.</typeparam>
        /// <param name="htmlHelper">The HTML Helper.</param>
        /// <returns>The sequence of the <see cref="AlertViewModel"/> class.</returns>
        public static IEnumerable<AlertViewModel> GetAlertViewModels<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var page = (htmlHelper.ViewDataContainer as WebViewPage);
            var items = (page?.TempData[AlertConfiguration.AlertTempDataKey] as IEnumerable<AlertViewModel>);
            
            return items ?? Enumerable.Empty<AlertViewModel>();
        }
    }
}
