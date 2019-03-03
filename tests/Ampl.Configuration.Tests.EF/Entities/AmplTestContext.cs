using System.Data.Entity;

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
