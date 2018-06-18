namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVideoConertationLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_VideoConvertationLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MediaId = c.Int(nullable: false),
                        Result = c.Boolean(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Uintra_VideoConvertationLog");
        }
    }
}
