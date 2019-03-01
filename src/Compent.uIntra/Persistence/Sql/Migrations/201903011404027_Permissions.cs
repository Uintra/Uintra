namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Permissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_Permission",
                c => new
                    {
                        IntranetMemberGroupId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                        ResourceTypeId = c.Int(nullable: false),
                        IsAllowed = c.Boolean(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.IntranetMemberGroupId, t.ActionId, t.ResourceTypeId })
                .Index(t => new { t.IntranetMemberGroupId, t.ActionId, t.ResourceTypeId }, unique: true, name: "UniqIndex");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Uintra_Permission", "UniqIndex");
            DropTable("dbo.Uintra_Permission");
        }
    }
}
