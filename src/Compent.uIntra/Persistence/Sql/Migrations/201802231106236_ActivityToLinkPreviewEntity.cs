namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ActivityToLinkPreviewEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_ActivityToLinkPreview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        LinkPreviewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id, name: "PK_ActivityToLinkPreview_Id")
                .ForeignKey("dbo.Uintra_Activity", t => t.ActivityId, name: "FK_ActivityToLinkPreview_Activity_Id")
                .Index(t => new { t.ActivityId, t.LinkPreviewId }, unique: true, name: "UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Uintra_ActivityToLinkPreview", "UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId");
            DropTable("dbo.Uintra_ActivityToLinkPreview");
        }
    }
}
