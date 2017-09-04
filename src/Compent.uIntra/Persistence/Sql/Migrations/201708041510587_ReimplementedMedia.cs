namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReimplementedMedia : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.uIntra_Media");
        }
    }
}
