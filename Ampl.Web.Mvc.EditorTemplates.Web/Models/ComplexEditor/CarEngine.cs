using System.ComponentModel.DataAnnotations;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models.ComplexEditor
{
  public enum CarEngine
  {
    [Display(ResourceType = typeof(Resources.ComplexModels), Name = "CarEngine_Unknown")]
    Unknown = 0,

    [Display(ResourceType = typeof(Resources.ComplexModels), Name = "CarEngine_Gasoline")]
    Gasoline = 1,

    //[Display(ResourceType = typeof(Resources.ComplexModels), Name = "CarEngine_Diesel")]
    [Display(Name = "Diesel (not from resource)")]
    Diesel = 2,

    [Display(Name = "Pure electric (not from resource)")]
    Electric = 3,

    [Display(ResourceType = typeof(Resources.ComplexModels), Name = "CarEngine_HybridGasoline")]
    HybridGasoline = 4,

    [Display(ResourceType = typeof(Resources.ComplexModels), Name = "CarEngine_HybridDiesel")]
    HybridDiesel = 5,
  }
}