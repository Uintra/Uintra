namespace Uintra20.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddBulletins : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uintra_ActivityLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        Address = c.String(),
                        ShortAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ActivityId, unique: true, name: "UQ_ActivityLocation_ActivityId");
            
            CreateTable(
                "dbo.Uintra_ActivitySubscribeSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        CanSubscribe = c.Boolean(nullable: false),
                        SubscribeNotes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_ActivityToLinkPreview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        LinkPreviewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ActivityId, t.LinkPreviewId }, unique: true, name: "UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId");
            
            CreateTable(
                "dbo.Uintra_Comment",
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
                "dbo.Uintra_CommentToLinkPreview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Guid(nullable: false),
                        LinkPreviewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.CommentId, t.LinkPreviewId }, unique: true, name: "UQ_CommentToLinkPreview_CommentId_LinkPreviewId");
            
            CreateTable(
                "dbo.Uintra_GroupActivityRelation",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.GroupId, t.ActivityId }, unique: true, name: "UQ_GroupActivity_GroupId_ActivityId");
            
            CreateTable(
                "dbo.Uintra_GroupDocument",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        MediaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_GroupMember",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        MemberId = c.Guid(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.GroupId, t.MemberId }, unique: true, name: "UQ_GroupMember_GroupId_MemberId");
            
            CreateTable(
                "dbo.Uintra_Group",
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
                "dbo.Uintra_Activity",
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
                "dbo.Uintra_Media",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                        MediaIds = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_Like",
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
                "dbo.Uintra_LinkPreview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        OgDescription = c.String(),
                        ImageId = c.Guid(),
                        FaviconId = c.Guid(),
                        MediaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_MemberNotifiersSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MemberId = c.Guid(nullable: false),
                        NotifierType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_MigrationHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        CreateDate = c.DateTime(nullable: false),
                        Version = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_MyLink",
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
                "dbo.Uintra_Notification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReceiverId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsNotified = c.Boolean(nullable: false),
                        IsViewed = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        NotifierType = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "2")
                                },
                            }),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_NotificationSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActivityType = c.Int(nullable: false),
                        NotificationType = c.Int(nullable: false),
                        NotifierType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        JsonData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_Permission",
                c => new
                    {
                        IntranetMemberGroupId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                        ResourceTypeId = c.Int(nullable: false),
                        IsAllowed = c.Boolean(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.IntranetMemberGroupId, t.ActionId, t.ResourceTypeId })
                .Index(t => new { t.IntranetMemberGroupId, t.ActionId, t.ResourceTypeId }, unique: true, name: "UniqIndex");
            
            CreateTable(
                "dbo.Uintra_Reminder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActivityId = c.Guid(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uintra_Subscribe",
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
            
            CreateTable(
                "dbo.Uintra_UserTagRelation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserTagId = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserTagId, t.EntityId }, unique: true, name: "UQ_UserTagRelation_UserTagId_EntityId");
            
            CreateTable(
                "dbo.Uintra_VideoConvertationLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MediaId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Result = c.Boolean(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Uintra_UserTagRelation", "UQ_UserTagRelation_UserTagId_EntityId");
            DropIndex("dbo.Uintra_Subscribe", "UQ_Subscribe_UserId_ActivityId");
            DropIndex("dbo.Uintra_Permission", "UniqIndex");
            DropIndex("dbo.Uintra_Like", "UQ_Like_UserId_EntityId");
            DropIndex("dbo.Uintra_GroupMember", "UQ_GroupMember_GroupId_MemberId");
            DropIndex("dbo.Uintra_GroupActivityRelation", "UQ_GroupActivity_GroupId_ActivityId");
            DropIndex("dbo.Uintra_CommentToLinkPreview", "UQ_CommentToLinkPreview_CommentId_LinkPreviewId");
            DropIndex("dbo.Uintra_ActivityToLinkPreview", "UQ_ActivityToLinkPreview_ActivityId_LinkPreviewId");
            DropIndex("dbo.Uintra_ActivityLocation", "UQ_ActivityLocation_ActivityId");
            DropTable("dbo.Uintra_VideoConvertationLog");
            DropTable("dbo.Uintra_UserTagRelation");
            DropTable("dbo.Uintra_Subscribe");
            DropTable("dbo.Uintra_Reminder");
            DropTable("dbo.Uintra_Permission");
            DropTable("dbo.Uintra_NotificationSetting");
            DropTable("dbo.Uintra_Notification",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "NotifierType",
                        new Dictionary<string, object>
                        {
                            { "SqlDefaultValue", "2" },
                        }
                    },
                });
            DropTable("dbo.Uintra_MyLink");
            DropTable("dbo.Uintra_MigrationHistory");
            DropTable("dbo.Uintra_MemberNotifiersSetting");
            DropTable("dbo.Uintra_LinkPreview");
            DropTable("dbo.Uintra_Like");
            DropTable("dbo.Uintra_Media");
            DropTable("dbo.Uintra_Activity");
            DropTable("dbo.Uintra_Group");
            DropTable("dbo.Uintra_GroupMember");
            DropTable("dbo.Uintra_GroupDocument");
            DropTable("dbo.Uintra_GroupActivityRelation");
            DropTable("dbo.Uintra_CommentToLinkPreview");
            DropTable("dbo.Uintra_Comment");
            DropTable("dbo.Uintra_ActivityToLinkPreview");
            DropTable("dbo.Uintra_ActivitySubscribeSetting");
            DropTable("dbo.Uintra_ActivityLocation");
        }
    }
}
