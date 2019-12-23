
namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants
{
    public class UsersInstallationConstants
    {
        public class DocumentTypeNames
        {
            public const string ProfilePage = "Profile Page";
            public const string ProfileEditPage = "Profile Edit Page";
        }
        public class DocumentTypeAliases
        {
            public const string ProfilePage = "profilePage";
            public const string ProfileEditPage = "profileEditPage";
        }

        public class DocumentTypeIcons
        {
            public const string ProfilePage = "icon-user";
            public const string ProfileEditPage = "icon-umb-members";
        }

        public class DataTypeNames
        {
            public const string MemberUserPicker = "Member - User picker";
            public const string MemberChangesWatcher = "Member - Member Changes Watcher";
        }

        public class DataTypeAliases
        {
            public const string Member = "Member";
        }

        public class DataTypePropertyEditors
        {
            public const string MemberUserPicker = "custom.RelatedUserPicker";
            public const string MemberChangesWatcher = "MemberChangesWatcher";
        }

        public class DataTypePropertyNames
        {
            public const string ProfileFirstName = "First name";
            public const string ProfileLastName = "Last name";
            public const string ProfilePhoto = "Photo";
            public const string Phone = "Phone";
            public const string Department = "Department";
            public const string ProfileRelatedUser = "Related user";
            public const string MembershipMemberChangesWatcher = "Member Changes Watcher";
        }

        public class DataTypePropertyAliases
        {
            public const string ProfileFirstName = "firstName";
            public const string ProfileLastName = "lastName";
            public const string ProfilePhoto = "photo";
            public const string Phone = "phone";
            public const string Department = "department";
            public const string ProfileRelatedUser = "relatedUser";
            public const string MembershipMemberChangesWatcher = "memberChangesWatcher";
        }

        public class DataTypeTabAliases
        {
            public const string ProfileTabAlias = "Profile";
            public const string MembershipTabAlias = "Membership";
        }

        public class MemberGroups
        {
            public const string GroupUiPublisher = "UiPublisher";
            public const string GroupWebMaster = "WebMaster";
            public const string GroupUiUser = "UiUser";
        }

        public class DefaultMember
        {
            public const string Email = "admin@testmember.com";
            public const string Name = "admin";
            public const int UmbracoAdminUserId = 0;
            public const string Password = "qwerty1234";
        }
    }
}
