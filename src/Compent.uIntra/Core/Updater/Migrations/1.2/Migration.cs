using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._1._2.Steps;

namespace Compent.Uintra.Core.Updater.Migrations._1._2
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("1.2");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private TranslationUpdateData TranslationUpdateData { get; } = new TranslationUpdateData
        {
            Add = new Dictionary<string, string>
            {
                { "Login.VersionIdentificatorPrefix.lbl","v"},
                { "TopNavigation.UintraDocumentationLink.lnk","Uintra Help"},
                { "UserList.Confirm.Delete.Title", "Warning"},
                { "UserList.Confirm.Delete.Text", "Are you sure?"},
                { "UserList.Table.Role.lbl", "Role"},
                { "UserList.Table.Department.lbl", "Department"},
                { "UserList.Table.Phone.lbl", "Phone"},
                {"Profile.Department.lbl","Department"},
                {"Profile.Phone.lbl","Phone" },
                {"Profile.Department.Placeholder", "Department placeholder" },
                {"Profile.Phone.Placeholder", "Phone placeholder" }

            },
            Remove = new List<string>()
        };

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return new TranslationsUpdateStep(TranslationUpdateData);
                yield return Resolve<AddPhoneAndDepartmentToUserStep>();
                yield return Resolve<ChangeGroupMembersDefaulPanelStep>();
            }
        }
    }
}