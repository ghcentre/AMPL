using System.ComponentModel.DataAnnotations;

namespace Ampl.Configuration
{
    public interface IAppConfigEntity
  {
    [Required]
    string Key { get; set; }
    string Value { get; set; }
  }
}
