using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._1._3.Steps;

namespace Compent.Uintra.Core.Updater.Migrations._1._3
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("1.3");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private TranslationUpdateData TranslationUpdateData { get; } = new TranslationUpdateData
        {
            Add = new Dictionary<string, string>
            {
                { "EventsCreate.Owner.lbl","Owner"},
                { "EventsEdit.Owner.lbl","Owner"},
                { "GroupInfo.OwnerName.lbl","Owner"},
                { "NewsEdit.Owner.lbl","Owner"},
                { "NewsCreate.Owner.lbl","owner"},
                { "BulletinsEdit.AtLeastOneFieldMustBeFilled.lbl","At least one field must be filled"}
            },
            Update = new Dictionary<string, (string old, string update)>
            {

            },
            Remove = new List<string>
            {
                 "EventsCreate.Creator.lbl",
                 "EventsEdit.Creator.lbl",
                 "GroupInfo.CreatorName",
                 "NewsEdit.Creator.lbl",
                 "NewsCreate.Creator.lbl",
                 "buy HTTPS licence for this domain right?\n\n\n\n#CentralFeedList.Empty.lbl"
            }
        };

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return new TranslationsUpdateStep(TranslationUpdateData);
                yield return Resolve<CreateForbiddenErrorPageStep>();
                yield return Resolve<SetupDefaultMemberGroupsPermissionsStep>();
            }
        }
    }
}