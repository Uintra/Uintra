namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming : DbMigration
    {
        public override void Up()
        {
            RenameTable("uIntra_ActivitySubscribeSetting", "Uintra_ActivitySubscribeSetting");
            RenameTable("uIntra_ActivityLocation", "Uintra_ActivityLocation");
            RenameTable("uIntra_UserTagRelation", "Uintra_UserTagRelation");
            RenameTable("uIntra_Comment", "Uintra_Comment");
            RenameTable("uIntra_GroupActivityRelation", "Uintra_GroupActivityRelation");
            RenameTable("uIntra_GroupDocument", "Uintra_GroupDocument");
            RenameTable("uIntra_GroupMember", "Uintra_GroupMember");
            RenameTable("uIntra_Group", "Uintra_Group");
            RenameTable("uIntra_Activity", "Uintra_Activity");
            RenameTable("uIntra_Media", "Uintra_Media");
            RenameTable("uIntra_Like", "Uintra_Like");
            RenameTable("uIntra_Media", "Uintra_Media");
            RenameTable("uIntra_MemberNotifiersSetting", "Uintra_MemberNotifiersSetting");
            RenameTable("uIntra_LinkPreview", "Uintra_LinkPreview");
            RenameTable("uIntra_MigrationHistory", "Uintra_MigrationHistory");
            RenameTable("uIntra_CommentToLinkPreview", "Uintra_CommentToLinkPreview");
            RenameTable("uIntra_MyLink", "Uintra_MyLink");
            RenameTable("uIntra_Notification", "Uintra_Notification");
            RenameTable("uIntra_Reminder", "Uintra_Reminder");
            RenameTable("uIntra_Subscribe", "Uintra_Subscribe");
            RenameTable("uIntra_NotificationSetting", "Uintra_NotificationSetting");
        }

        public override void Down()
        {
            RenameTable("Uintra_ActivitySubscribeSetting", "uIntra_ActivitySubscribeSetting");
            RenameTable("Uintra_ActivityLocation", "uIntra_ActivityLocation");
            RenameTable("Uintra_UserTagRelation", "Uintra_uIerTagRelation");
            RenameTable("Uintra_Comment", "uIntra_Comment");
            RenameTable("Uintra_GroupActivityRelation", "uIntra_GroupActivityRelation");
            RenameTable("Uintra_GroupDocument", "uIntra_GroupDocument");
            RenameTable("Uintra_GroupMember", "uIntra_GroupMember");
            RenameTable("Uintra_Group", "uIntra_Group");
            RenameTable("Uintra_Activity", "uIntra_Activity");
            RenameTable("Uintra_Media", "uIntra_Media");
            RenameTable("Uintra_Like", "uIntra_Like");
            RenameTable("Uintra_Media", "uIntra_Media");
            RenameTable("Uintra_MemberNotifiersSetting", "uIntra_MemberNotifiersSetting");
            RenameTable("Uintra_LinkPreview", "uIntra_LinkPreview");
            RenameTable("Uintra_MigrationHistory", "uIntra_MigrationHistory");
            RenameTable("Uintra_CommentToLinkPreview", "uIntra_CommentToLinkPreview");
            RenameTable("Uintra_MyLink", "uIntra_MyLink");
            RenameTable("Uintra_Notification", "uIntra_Notification");
            RenameTable("Uintra_Reminder", "uIntra_Reminder");
            RenameTable("Uintra_Subscribe", "uIntra_Subscribe");
            RenameTable("Uintra_NotificationSetting", "uIntra_NotificationSetting");
        }
    }
}
