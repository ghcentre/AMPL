using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class DropDownListEditorViewModel
  {
    [Display(Name = "Short Value",
             Description = "Please enter the integer value (positive or negative) from -32768 to 32767",
             Prompt = "Example: 0",
             ShortName = "Short Val")]
    [DropDownList("GetColors")]
    public int Color1 { get; set; }

    public static IEnumerable<SelectListItem> GetColors()
    {
      return typeof(Color)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Where(x => x.ReflectedType == typeof(Color))
        .Select(x => (Color)x.GetValue(null))
        .Select(x => new SelectListItem() { Text = x.Name, Value = x.ToArgb().ToString() });
    }
  }
}