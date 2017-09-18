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
                        MemberId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id, name: "PK_uIntra_GroupMember_Id")
                .Index(t => new { t.GroupId, t.MemberId }, unique: true, name: "UQ_GroupMember_GroupId_MemberId");
            
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
                        ImageId = c.Int(),
                        IsHidden = c.Boolean(nullable: false),
                        GroupTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.uIntra_GroupMember", "UQ_GroupMember_GroupId_MemberId");
            DropTable("dbo.uIntra_Group");
            DropTable("dbo.uIntra_GroupMember");
            DropTable("dbo.uIntra_GroupDocument");
        }
    }
}
