using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ampl.System;
using Ampl.Web.Mvc.EditorTemplates.Web.Models;
using Newtonsoft.Json;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
  public class ComplexEditorController : ModelTesterControllerBase
  {
    private static Dictionary<string, object> _models = new Dictionary<string, object>();

    public ActionResult Index()
    {
      return View();
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

    public ActionResult DateTimeEditor(bool createModel)
    {
      return HandleGetAction(createModel, () => new DateTimeEditorViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DateTimeEditor(DateTimeEditorViewModel model)
    {
      return HandlePostAction(model);
    }

    public ActionResult DropDownListEditor(bool createModel)
    {
      //global::System.Threading.Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("ru-RU");
      return HandleGetAction(createModel, () => new DropDownListEditorViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DropDownListEditor(DropDownListEditorViewModel model)
    {
      return HandlePostAction(model);
    }

    private IEnumerable<CollectionEditorViewModel.BranchInfo> GetBranches()
    {
      var wc = new WebClient() { Encoding = Encoding.UTF8 };
      return wc.DownloadString("https://www.artlebedev.ru/tools/country-list/tab/")
                .Replace("\r", string.Empty)
                .Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split('\t'))
                .Where(x => x.Length >= 8 && x[7] == "Западная Европа")
                .Select(x => new CollectionEditorViewModel.BranchInfo() {
                  CountryName = x[2],
                  CountryCode = x[3],
                  NumberOfEmployees = null
                });
    }

    public ActionResult FixedCollectionEditor(bool createModel)
    {
      return HandleGetAction(
        createModel,
        () => new FixedCollectionEditorViewModel() {
                    Branches = GetBranches(),
                    Ints = new int[5],
                    NullableInts = new int?[5],
                    NullablesRequired = Enumerable.Range(0, 3)
                                          .Select(x => new FixedCollectionEditorViewModel.NullableRequired())
                  });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult FixedCollectionEditor(FixedCollectionEditorViewModel model)
    {
      TempData["EditorTemplateConfiguration"] = new EditorTemplateConfiguration() {
        MaximumTemplateDepth = 1
      };
      return HandlePostAction(model);
    }

    public ActionResult EditableCollectionEditor(bool createModel, bool generateInitialCollection = false)
    {
      return HandleGetAction(
        createModel,
        () => new EditableCollectionEditorViewModel() { Branches = generateInitialCollection ? GetBranches() : null }
      );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditableCollectionEditor(EditableCollectionEditorViewModel model)
    {
      TempData["EditorTemplateConfiguration"] = new EditorTemplateConfiguration() { MaximumTemplateDepth = 3 };
      return HandlePostAction(model);
    }
  }
}
