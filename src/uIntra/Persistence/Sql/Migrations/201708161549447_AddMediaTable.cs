using System.Data.Entity.Migrations;

namespace uIntra.Persistence.Sql.Migrations
{
    public partial class AddMediaTable : DbMigration
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
