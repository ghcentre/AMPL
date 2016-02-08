namespace Ampl.Configuration.Tests.EntityFramework.Migrations
{
  using System;
  using global::System.Data.Entity.Migrations;

  public partial class M002 : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.AppConfigKeyResolvers",
          c => new {
            FromKey = c.String(nullable: false, maxLength: 400),
            ToKey = c.String(nullable: false, maxLength: 400),
          })
          .PrimaryKey(t => t.FromKey);

    }

    public override void Down()
    {
      DropTable("dbo.AppConfigKeyResolvers");
    }
  }
}
