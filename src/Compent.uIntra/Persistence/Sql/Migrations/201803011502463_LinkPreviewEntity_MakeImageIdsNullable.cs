namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkPreviewEntity_MakeImageIdsNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Uintra_LinkPreview", "ImageId", c => c.Guid());
            AlterColumn("dbo.Uintra_LinkPreview", "FaviconId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Uintra_LinkPreview", "FaviconId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Uintra_LinkPreview", "ImageId", c => c.Guid(nullable: false));
        }
    }
}
