namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_UserTagRelation",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserTagId = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserTagId, t.EntityId }, unique: true, name: "UQ_UserTagRelation_UserTagId_EntityId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.uIntra_UserTagRelation", "UQ_UserTagRelation_UserTagId_EntityId");
            DropTable("dbo.uIntra_UserTagRelation");
        }
    }
}
