namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupActivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_GroupActivity",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.GroupId, t.ActivityId }, unique: true, name: "UQ_GroupActivity_GroupId_ActivityId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.uIntra_GroupActivity", "UQ_GroupActivity_GroupId_ActivityId");
            DropTable("dbo.uIntra_GroupActivity");
        }
    }
}
