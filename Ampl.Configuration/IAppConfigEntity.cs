using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  public interface IAppConfigEntity
  {
    [Required]
    string Key { get; set; }
    string Value { get; set; }
  }
}
