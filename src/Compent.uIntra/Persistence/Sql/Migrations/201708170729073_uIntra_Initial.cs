namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class uIntra_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.uIntra_Comment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(nullable: false),
                        Text = c.String(),
                        ParentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_GroupActivityRelation",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.GroupId, t.ActivityId }, unique: true, name: "UQ_GroupActivity_GroupId_ActivityId");
            
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
                .PrimaryKey(t => t.Id)
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
            
            CreateTable(
                "dbo.uIntra_Activity",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        JsonData = c.String(),
                        Type = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_Media",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                        MediaIds = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_Like",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserId, t.EntityId }, unique: true, name: "UQ_Like_UserId_EntityId");
            
            CreateTable(
                "dbo.uIntra_MemberNotifiersSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MemberId = c.Guid(nullable: false),
                        NotifierType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.uIntra_MyLink",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ContentId = c.Int(nullable: false),
                        QueryString = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActivityId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_Notification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReceiverId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsNotified = c.Boolean(nullable: false),
                        IsViewed = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_Reminder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.uIntra_Subscribe",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsNotificationDisabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserId, t.ActivityId }, unique: true, name: "UQ_Subscribe_UserId_ActivityId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.uIntra_Subscribe", "UQ_Subscribe_UserId_ActivityId");
            DropIndex("dbo.uIntra_Like", "UQ_Like_UserId_EntityId");
            DropIndex("dbo.uIntra_GroupMember", "UQ_GroupMember_GroupId_MemberId");
            DropIndex("dbo.uIntra_GroupActivityRelation", "UQ_GroupActivity_GroupId_ActivityId");
            DropTable("dbo.uIntra_Subscribe");
            DropTable("dbo.uIntra_Reminder");
            DropTable("dbo.uIntra_Notification");
            DropTable("dbo.uIntra_MyLink");
            DropTable("dbo.uIntra_MigrationHistory");
            DropTable("dbo.uIntra_MemberNotifiersSetting");
            DropTable("dbo.uIntra_Like");
            DropTable("dbo.uIntra_Media");
            DropTable("dbo.uIntra_Activity");
            DropTable("dbo.uIntra_Group");
            DropTable("dbo.uIntra_GroupMember");
            DropTable("dbo.uIntra_GroupDocument");
            DropTable("dbo.uIntra_GroupActivityRelation");
            DropTable("dbo.uIntra_Comment");
        }
    }
}
