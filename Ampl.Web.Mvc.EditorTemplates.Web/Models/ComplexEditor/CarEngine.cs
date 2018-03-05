using System.ComponentModel.DataAnnotations;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models.ComplexEditor
{
  public enum CarEngine
  {
    [Display(Name = "Unknown engine")]
    Unknown = 0,

    [Display(Name = "Gasoline engine")]
    Gasoline = 1,

    [Display(Name = "Diesel engine")]
    Diesel = 2,

    [Display(Name = "Pure electric")]
    Electric = 3,

    [Display(Name = "Electric + Gasoline Hybrid engine")]
    HybridGasoline = 3,

    [Display(Name = "Electric + Diesel Hybrid engine")]
    HybridDiesel = 4,
  }
}