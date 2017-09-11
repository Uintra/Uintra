namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Groups : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.uIntra_Group");
        }
    }
}
