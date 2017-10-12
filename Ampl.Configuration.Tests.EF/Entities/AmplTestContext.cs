using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration.Tests.EF.Entities
{
  public class AmplTestContext : DbContext
  {
    public AmplTestContext() : base("AmplTest")
    { }

    public virtual DbSet<AppConfigItem> AppConfigItems { get; set; }
    public virtual DbSet<AppConfigKeyResolver> AppConfigKeyResolvers { get; set; }
  }
}
