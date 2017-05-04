using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ampl.Web.Mvc.EditorTemplates.Web.Models;
using Newtonsoft.Json;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
  public class ComplexEditorController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    private ActionResult HandleGetAction<T>(bool createModel, Func<T> modelFactory)
    {
      if(createModel)
      {
        return View(modelFactory());
      }
      else
      {
        return View();
      }
    }

    private ActionResult HandlePostAction<T>(T model)
    {
      if(!ModelState.IsValid)
      {
        this.AddAlert("ModelState is not valid.", ViewModels.AlertContextualClass.Warning, "Validation", true);
        return View(model);
      }

      this.AddAlert(new HtmlString($"The model is: <pre>{JsonConvert.SerializeObject(model, Formatting.Indented)}</pre>").ToHtmlString());
      return RedirectToAction("Index");
    }

    public ActionResult NumericEditor(bool createModel)
    {
      return HandleGetAction(createModel, () => new NumericEditorViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult NumericEditor(NumericEditorViewModel model)
    {
      return HandlePostAction(model);
    }

    public ActionResult DropDownListEditor(bool createModel)
    {
      return HandleGetAction(createModel, () => new DropDownListEditorViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DropDownListEditor(DropDownListEditorViewModel model)
    {
      return HandlePostAction(model);
    }

  }
}
