namespace Ampl.Configuration.Tests.EntityFramework.Migrations
{
  using global::System.Data.Entity.Migrations;

  internal sealed class Configuration : DbMigrationsConfiguration<Ampl.Configuration.Tests.EntityFramework.AmplTestContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(Ampl.Configuration.Tests.EntityFramework.AmplTestContext context)
    {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
    }
  }
}
