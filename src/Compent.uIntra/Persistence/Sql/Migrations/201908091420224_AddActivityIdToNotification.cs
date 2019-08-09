namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityIdToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                table:"dbo.Uintra_Notification", 
                name:"ActivityId", 
                columnAction:c => c.Guid(
                    nullable: false, 
                    defaultValue: default(Guid))
            );
        }
        
        public override void Down()
        {
            DropColumn(
                table:"dbo.Uintra_Notification", 
                name:"ActivityId"
            );
        }
    }
}
