namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissionTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_PermissionActivityType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Activity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_Permission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PermissionActivityTypeId = c.Guid(),
                        IntranetMemberGroupId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            AddForeignKey("dbo.Uintra_Permission", "PermissionActivityTypeId", "dbo.Uintra_PermissionActivityType", "Id", name: "FK_Uintra_Permission_PermissionActivityTypeId_Uintra_PermissionActivityType");

        }
        
        public override void Down()
        {
            DropTable("dbo.Uintra_Permission");
            DropTable("dbo.Uintra_PermissionActivityType");

            DropForeignKey("dbo.Uintra_Permission", "FK_Uintra_Permission_PermissionActivityTypeId_Uintra_PermissionActivityType");
        }
    }
}
