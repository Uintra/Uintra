namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkPreviewEntity_AddMediaIdForLocalRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uintra_LinkPreview", "MediaId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uintra_LinkPreview", "MediaId");
        }
    }
}
