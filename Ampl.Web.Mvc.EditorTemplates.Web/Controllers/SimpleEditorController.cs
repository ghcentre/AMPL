using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
  public class SimpleEditorController : Controller
  {
    // GET: SimpleTypes
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult StringEditor()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult StringEditor(string model)
    {
      if(!ModelState.IsValid)
      {
        this.AddAlert("ModelState is not valid.", ViewModels.AlertContextualClass.Warning, "Validation", true);
        return View(model);
      }

      this.AddAlert("The model validated successfully.");
      return RedirectToAction("Index");
    }
  }
}