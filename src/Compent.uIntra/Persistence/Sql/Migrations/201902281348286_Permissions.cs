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
                        Id = c.Guid(nullable: false),
                        IntranetMemberGroupId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                        ResourceTypeId = c.Int(nullable: false),
                        IsAllowed = c.Boolean(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Uintra_Permission");
        }
    }
}
