using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class CollectionEditorViewModel
  {
    [Required]
    [Display(Name = "Document Title", Order = 10000)]
    public string DocumentTitle { get; set; }

    public class BranchInfo
    {
      [Required]
      [Display(ResourceType = typeof(Resources.ComplexModels), Name = "BranchInfo_CountryName")]
      public string Title { get; set; }

      [Required]
      [StringLength(2, MinimumLength = 2)]
      public string CountryCode { get; set; }

      [Display(Name = "Number of employees", Description = "Enter number of leave empty if no branch in that country.")]
      public int? NumberOfEmployees { get; set; }
    }
  }
}