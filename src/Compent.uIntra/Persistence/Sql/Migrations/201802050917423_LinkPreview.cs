namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class LinkPreview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.uIntra_LinkPreview",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        OgDescription = c.String(),
                        ImageId = c.Guid(nullable: false),
                        FaviconId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.uIntra_CommentToLinkPreview",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Guid(nullable: false),
                        LinkPreviewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.uIntra_Comment", t => t.CommentId, name: "FK_CommentToLinkPreview_Comment_Id")
                .ForeignKey("dbo.uIntra_LinkPreview", t => t.LinkPreviewId, name: "FK_CommentToLinkPreview_LinkPreview_Id")
                .Index(t => new { t.CommentId, t.LinkPreviewId }, unique: true, name: "UQ_CommentToLinkPreview_CommentId_LinkPreviewId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.uIntra_CommentToLinkPreview", "CommentId", "dbo.uIntra_Comment");
            DropForeignKey("dbo.uIntra_CommentToLinkPreview", "LinkPreviewId", "dbo.uIntra_LinkPreview");

            DropIndex("dbo.CommentToLinkPreviewEntities", "UQ_CommentToLinkPreview_CommentId_LinkPreviewId");
            DropTable("dbo.uIntra_LinkPreview");
            DropTable("dbo.CommentToLinkPreviewEntities");
        }
    }
}
