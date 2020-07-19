using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ampl.Web.Mvc.ViewModels
{
    /// <summary>
    /// Represents the alert contextual class.
    /// </summary>
    public enum AlertContextualClass
    {
        /// <summary>
        /// Success.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Information.
        /// </summary>
        Info,

        /// <summary>
        /// Warning.
        /// </summary>
        Warning,

        /// <summary>
        /// Danger.
        /// </summary>
        Danger,
    }

    /// <summary>
    /// Represents the Alert View Model.
    /// </summary>
    public sealed class AlertViewModel
    {
        /// <summary>
        /// Alert Contextual Class.
        /// </summary>
        public AlertContextualClass ContextalClass { get; set; }

        /// <summary>
        /// Heading.
        /// </summary>
        public IHtmlString Heading { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public IHtmlString Text { get; set; }

        /// <summary>
        /// Is Alert dismissible.
        /// </summary>
        public bool Dismissible { get; set; }
    }
}
