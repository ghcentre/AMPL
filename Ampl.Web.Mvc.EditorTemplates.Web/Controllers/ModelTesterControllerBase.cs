using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ampl.Core;
using Ampl.System;
using Newtonsoft.Json;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
  public class ModelTesterControllerBase : Controller
  {
    private static Dictionary<string, object> _models = new Dictionary<string, object>();

    public ModelTesterControllerBase() : base()
    { }

    protected ActionResult HandleGetAction<T>(bool createModel, Func<T> modelFactory)
    {
      return createModel ? View(modelFactory()) : View();
    }

    protected ActionResult HandlePostAction<T>(T model)
    {
      if(!ModelState.IsValid)
      {
        this.AddAlert("ModelState is not valid.", ViewModels.AlertContextualClass.Warning, "Validation", true);
        return View(model);
      }

      string key = Guid.NewGuid().ToString();
      _models.Add(key, model);
      this.AddAlert("ModelState is valid.");
      return RedirectToAction("Details", new { id = key });
    }

    public ActionResult Details(string id)
    {
      object model = null;
      if(id.ToNullIfWhiteSpace() == null || !_models.TryGetValue(id, out model))
      {
        this.AddAlert("Model not found", ViewModels.AlertContextualClass.Danger);
        return RedirectToAction("Index");
      }

      ViewBag.ID = id;
      return View(model);
    }

    public ActionResult JsonPrint(string id)
    {
      object theObject = null;
      if(id.ToNullIfWhiteSpace() == null || !_models.TryGetValue(id, out theObject))
      {
        this.AddAlert("Model not found", ViewModels.AlertContextualClass.Danger);
        return RedirectToAction("Index");
      }

      string model = JsonConvert.SerializeObject(theObject, Formatting.Indented);
      ViewBag.ID = id;
      return View((object)model);
    }
  }
}