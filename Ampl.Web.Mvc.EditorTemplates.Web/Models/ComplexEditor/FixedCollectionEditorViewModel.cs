using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class FixedCollectionEditorViewModel : CollectionEditorViewModel
  {
    [Display(Order = 20000)]
    public IEnumerable<BranchInfo> Branches { get; set; }

    public IEnumerable<int> Ints { get; set; }

    public IEnumerable<int?> NullableInts { get; set; }

    public class NullableRequired
    {
      [Required]
      [Display(Name = "Null Req")]
      public int? RequiredValue { get; set; }
    }

    public IEnumerable<NullableRequired> NullablesRequired { get; set; }
  }
}