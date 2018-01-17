namespace Compent.uIntra.Persistence.Sql.Migrations
{
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
                .PrimaryKey(t => t.Id, name: "PK_uIntra_ActivitySubscribeSetting_Id")
                .ForeignKey("dbo.uIntra_Activity", c => c.ActivityId, name: "FK_uIntra_ActivitySubscribeSetting_ActivityId_uIntra_Activity_Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropTable("dbo.uIntra_ActivitySubscribeSetting");
        }
    }
}
