using Ampl.Web.Mvc.EditorTemplates.Web.Models.ComplexEditor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models.ComplexView
{
    public class ComplexViewModel
    {

        [Display(Name = "Title", Description = "This is model title. It should be displayed (not the page title)")]
        [DisplayFormat(NullDisplayText = "(no title)")]
        public string Title { get; set; }


        [Display(Name = "Color 1", Description = "Required value, items from static method")]
        [DropDownList("GetColorsStatic")]
        [DisplayFormat(NullDisplayText = "(color not selected)")]
        public int Color1 { get; set; }

        public static IEnumerable<SelectListItem> GetColorsStatic()
        {
            return typeof(Color)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.ReflectedType == typeof(Color))
                .Select(x => (Color)x.GetValue(null))
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.ToArgb().ToString() });
        }

        [Display(Name = "Color 1", Description = "Required value, items from instance method")]
        [DropDownList("GetColorsInstance")]
        public int Color2 { get; set; }

        public IEnumerable<SelectListItem> GetColorsInstance() =>
            GetColorsStatic();

        [Display(Name = "Color 3", Description = "Required value, items from static get property")]
        [DropDownList("ColorsStatic")]
        public int Color3 { get; set; }

        public static IEnumerable<SelectListItem> ColorsStatic =>
            GetColorsStatic();

        [Display(Name = "Color 4", Description = "Required value, items from instance get property")]
        [DropDownList("ColorsInstance")]
        public int Color4 { get; set; }

        [ScaffoldColumn(false)]
        [JsonIgnore]
        public IEnumerable<SelectListItem> ColorsInstance =>
            GetColorsStatic();

        [Display(Name = "Color 5", Description = "Nullable value")]
        [DropDownList("GetColorsStatic")]
        [DisplayFormat(NullDisplayText = "(custom NullDisplayText -- color not selected)")]
        public int? Color5 { get; set; }

        [Display(Name = "Car Engine", Description = "DropDownList for non-null Enum Type.")]
        [DropDownList(typeof(CarEngine))]
        public CarEngine CarEngine { get; set; }

        [Display(Name = "Car Engine 2", Description = "DropDownList for null Enum Type.")]
        [DropDownList(typeof(CarEngine))]
        public CarEngine? CarEngine2 { get; set; }
    }
}