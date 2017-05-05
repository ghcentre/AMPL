using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class EditableCollectionEditorViewModel : CollectionEditorViewModel
  {
    [Display(Order = 20000)]
    [UIHint("EditableCollection")]
    public IEnumerable<BranchInfo> Branches { get; set; }
  }
}