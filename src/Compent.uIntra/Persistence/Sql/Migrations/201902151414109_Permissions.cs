namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Permissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_PermissionActivityType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        PermissionEntityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_Permission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IntranetMemberGroupId = c.Int(nullable: false),
                        IntranetActionId = c.Int(nullable: false),
                        IsAllowed = c.Boolean(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Uintra_Permission");
            DropTable("dbo.Uintra_PermissionActivityType");
        }
    }
}
