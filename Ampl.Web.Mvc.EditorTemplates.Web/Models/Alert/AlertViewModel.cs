using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ampl.Web.Mvc.ViewModels;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class AlertViewModel
  {
    [DropDownList("GetContextualClasses")]
    public AlertContextualClass ContextalClass { get; set; }

    public IEnumerable<SelectListItem> GetContextualClasses() =>
      Enum.GetNames(typeof(AlertContextualClass))
        .Select(x => new SelectListItem() { Text = x, Value = x });

    public bool Dismissible { get; set; }
    public string Heading { get; set; }
    public string Text { get; set; }

  }
}