using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
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
                { "UserList.Table.Email.lbl", "Email"},
                { "Profile.Department.lbl","Department"},
                { "Profile.Phone.lbl","Phone" },
                { "Profile.Email.lbl","E-mail" },
                { "Profile.Title.lbl","Edit profile" },
                { "Profile.Department.Placeholder", "Department placeholder" },
                { "Profile.Phone.Placeholder", "Phone placeholder" },
                { "Profile.Overview.Title.lbl", "Profile" },
                { "Profile.Overview.Department.lbl", "Department:"},
                { "Profile.Overview.Phone.lbl", "Phone:" },
                { "Profile.Overview.Email.lbl", "E-mail:" },
                { "UserList.Table.GroupAdmin.lbl", "Group admin" },
                { "UserList.Table.GroupMember.lbl", "Group member" }
            },
            Update = new Dictionary<string, (string old, string update)>
            {
                { "SystemLinks.Menu.lbl", ("System Links Block", "Shared Links Block") },
                { "FileUploadView.UploadFiles.lbl", ("Drop files and images here or click to upload.", "Insert image") }
            },
            Remove = new List<string>()
            {
                { "GroupRoom.Label" },
                { "Profile.UploadNewPhoto.lbl" }
            }
        };

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return new TranslationsUpdateStep(TranslationUpdateData);
                //yield return SplitEventLabels.SplitEventLabelsTranslationsUpdateStep();
                yield return Resolve<AddPhoneAndDepartmentToUserStep>();
                yield return Resolve<ChangeGroupMembersDefaulPanelStep>();
                yield return Resolve<UseInSearchDefaultTrueStep>();
                yield return Resolve<ChangeSystemLinksToSharedLinksStep>();
            }
        }
    }
}