using System.ComponentModel.DataAnnotations;

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
