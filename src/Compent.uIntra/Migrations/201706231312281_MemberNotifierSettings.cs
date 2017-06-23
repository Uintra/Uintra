namespace Compent.uIntra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberNotifierSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberNotifiersSetting",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MemberId = c.Guid(nullable: false),
                        NotifierType = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MemberNotifiersSetting");
        }
    }
}
