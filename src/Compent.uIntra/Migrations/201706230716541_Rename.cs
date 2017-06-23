namespace Compent.uIntra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Comment", newName: "uIntra_Comment");
            RenameTable(name: "dbo.Activity", newName: "uIntra_Activity");
            RenameTable(name: "dbo.Like", newName: "uIntra_Like");
            RenameTable(name: "dbo.MyLink", newName: "uIntra_MyLink");
            RenameTable(name: "dbo.Notification", newName: "uIntra_Notification");
            RenameTable(name: "dbo.Reminder", newName: "uIntra_Reminder");
            RenameTable(name: "dbo.Subscribe", newName: "uIntra_Subscribe");
            RenameTable(name: "dbo.Tag", newName: "uIntra_Tag");
            RenameTable(name: "dbo.TagActivityRelation", newName: "uIntra_TagActivityRelation");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.uIntra_Subscribe", newName: "Subscribe");
            RenameTable(name: "dbo.uIntra_Reminder", newName: "Reminder");
            RenameTable(name: "dbo.uIntra_Notification", newName: "Notification");
            RenameTable(name: "dbo.uIntra_MyLink", newName: "MyLink");
            RenameTable(name: "dbo.uIntra_Like", newName: "Like");
            RenameTable(name: "dbo.uIntra_Activity", newName: "Activity");
            RenameTable(name: "dbo.uIntra_Comment", newName: "Comment");
            RenameTable(name: "dbo.uIntra_Tag", newName: "Tag");
            RenameTable(name: "dbo.uIntra_TagActivityRelation", newName: "TagActivityRelation");
        }
    }
}
