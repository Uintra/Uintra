using Uintra.Core.Extensions;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Persistence.Sql.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifierTypeWithDefaultToNotificationsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uintra_Notification", "NotifierType", c => c.Int(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: NotifierTypeEnum.UiNotifier.ToInt().ToString())
                    },
                }));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uintra_Notification", "NotifierType",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "SqlDefaultValue", NotifierTypeEnum.UiNotifier.ToInt().ToString() },
                });
        }
    }
}
