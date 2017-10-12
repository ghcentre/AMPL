  using System;
  using System.Data.Entity.Migrations;
namespace Ampl.Configuration.Tests.EF.Migrations
{

  public partial class _001 : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.AppConfigItems",
          c => new {
            Key = c.String(nullable: false, maxLength: 400),
            Value = c.String(),
            AnotherField = c.DateTime(nullable: false),
          })
          .PrimaryKey(t => t.Key);

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
      DropTable("dbo.AppConfigItems");
    }
  }
}
