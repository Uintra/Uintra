namespace Compent.uIntra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_ActivityLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.uIntra_ActivityLocation",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Guid(nullable: false),
                        Address = c.String(),
                        ShortAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id, name: "PK_ActivityLocation_Id")
                .ForeignKey("dbo.uIntra_Activity", t => t.ActivityId, name: "FK_ActivityLocation_Activity_Id")
                .Index(t => t.ActivityId, unique: true, name: "UQ_ActivityLocation_ActivityId");

        }

        public override void Down()
        {
            DropForeignKey("dbo.uIntra_ActivityLocation", "ActivityId", "dbo.uIntra_Activity");
            DropIndex("dbo.uIntra_ActivityLocation", "UQ_ActivityLocation_ActivityId");
            DropTable("dbo.uIntra_ActivityLocation");
        }
    }
}
