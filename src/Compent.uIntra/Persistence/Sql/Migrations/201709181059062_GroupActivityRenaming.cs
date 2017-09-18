namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupActivityRenaming : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.uIntra_GroupActivity", newName: "uIntra_GroupActivityRelation");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.uIntra_GroupActivityRelation", newName: "uIntra_GroupActivity");
        }
    }
}
