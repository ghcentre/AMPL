using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ampl.Web.Mvc.EditorTemplates.Web.Models;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
  public class AlertController : Controller
  {
    public ActionResult Index()
    {
      return View(new AlertViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(AlertViewModel model)
    {
      if(!ModelState.IsValid)
      {
        return View(model);
      }

      this.AddAlert(model.Text, model.ContextalClass, model.Heading, model.Dismissible);
      return RedirectToAction("Index", "Home");
    }
  }
}