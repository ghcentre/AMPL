using System;
using System.ComponentModel.DataAnnotations;

namespace Ampl.Configuration.Tests.EF.Entities
{
    public class AppConfigItem : IAppConfigEntity
    {
        [StringLength(400)]
        [Required]
        [Key]
        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        [DataType(DataType.DateTime)]
        public virtual DateTime AnotherField { get; set; } = DateTime.Now;
    }
}
