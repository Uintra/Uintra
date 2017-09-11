namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Groups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_GroupDocument",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        MediaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_GroupMember",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        MemeberId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.GroupId, t.MemeberId }, unique: true, name: "UQ_GroupMember_GroupId_MemeberId");
            
            CreateTable(
                "dbo.uIntra_Group",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                        ParentActivityId = c.Guid(),
                        ImageId = c.Int(),
                        IsHidden = c.Boolean(nullable: false),
                        GroupTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.uIntra_GroupMember", "UQ_GroupMember_GroupId_MemeberId");
            DropTable("dbo.uIntra_Group");
            DropTable("dbo.uIntra_GroupMember");
            DropTable("dbo.uIntra_GroupDocument");
        }
    }
}
