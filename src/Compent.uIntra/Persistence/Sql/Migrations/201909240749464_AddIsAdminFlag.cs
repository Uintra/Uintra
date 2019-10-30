namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAdminFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uintra_GroupMember", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uintra_GroupMember", "IsAdmin");
        }
    }
}
