using Uintra.Core.Extensions;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddNotifierTypeToNotificationsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uintra_Notification", "NotifierType", c => c.Int(nullable: false, defaultValueSql: NotifierTypeEnum.UiNotifier.ToInt().ToString()));
        }

        public override void Down()
        {
            DropColumn("dbo.Uintra_Notification", "NotifierType");
        }
    }
}
