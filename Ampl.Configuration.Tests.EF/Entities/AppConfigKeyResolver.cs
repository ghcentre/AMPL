using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration.Tests.EF.Entities
{
  public class AppConfigKeyResolver
  {
    [StringLength(400)]
    [Required]
    [Key]
    public virtual string FromKey { get; set; }

    [StringLength(400)]
    [Required]
    public virtual string ToKey { get; set; }
  }
}
