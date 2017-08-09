namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Merge_master_into_Release_12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_MigrationHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        CreateDate = c.DateTime(nullable: false),
                        Version = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.uIntra_MyLink", "ActivityId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.uIntra_MyLink", "ActivityId");
            DropTable("dbo.uIntra_MigrationHistory");
        }
    }
}
