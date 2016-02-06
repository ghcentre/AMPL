namespace Ampl.Configuration.Tests.EntityFramework.Migrations
{
  using System;
  using global::System.Data.Entity.Migrations;

  public partial class M001 : DbMigration
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

    }

    public override void Down()
    {
      DropTable("dbo.AppConfigItems");
    }
  }
}
