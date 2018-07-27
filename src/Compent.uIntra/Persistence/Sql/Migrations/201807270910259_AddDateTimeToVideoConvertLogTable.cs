namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeToVideoConvertLogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uintra_VideoConvertationLog", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uintra_VideoConvertationLog", "Date");
        }
    }
}
