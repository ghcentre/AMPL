using Ampl.Web.Mvc.EditorTemplates.Web.Models.ComplexView;
using System.Drawing;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Controllers
{
    public class ComplexViewController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ComplexView(bool useModel = true)
        {
            ComplexViewModel model = null;

            if (useModel)
            {
                model = new ComplexViewModel() {
                    Title = "Model title",
                    Color1 = Color.Red.ToArgb(),
                    Color2 = Color.Green.ToArgb(),
                    Color3 = Color.Blue.ToArgb(),
                    Color4 = Color.Black.ToArgb(),
                    Color5 = Color.Gray.ToArgb(),
                    CarEngine = Models.ComplexEditor.CarEngine.Diesel,
                    CarEngine2 = Models.ComplexEditor.CarEngine.Gasoline,
                };
            }

            return View(model);
        }
    }
}