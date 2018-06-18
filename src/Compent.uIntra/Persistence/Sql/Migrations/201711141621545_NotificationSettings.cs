namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_NotificationSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActivityType = c.Int(nullable: false),
                        NotificationType = c.Int(nullable: false),
                        NotifierType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        JsonData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.uIntra_NotificationSetting");
        }
    }
}
