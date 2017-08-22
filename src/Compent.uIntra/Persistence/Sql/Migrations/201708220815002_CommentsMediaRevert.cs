namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsMediaRevert : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.uIntra_Media");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.uIntra_Media",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                        MediaIds = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
