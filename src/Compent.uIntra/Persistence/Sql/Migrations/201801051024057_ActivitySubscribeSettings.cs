namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivitySubscribeSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_ActivitySubscribeSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        CanSubscribe = c.Boolean(nullable: false),
                        SubscribeNotes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.uIntra_ActivitySubscribeSetting");
        }
    }
}
