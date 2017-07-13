namespace uIntra.Users.Installer
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
        }

        public class DataTypeAliases
        {
            public const string Member = "Member";
        }
        public class DataTypePropertyEditors
        {
            public const string MemberUserPicker = "Umbraco.UserPicker";
        }

        public class DataTypePropertyNames
        {
            public const string ProfileFirstName = "First name";
            public const string ProfileLastName = "Last name";
            public const string ProfilePhoto = "Photo";
            public const string ProfileRelatedUser = "Related user";
        }
        public class DataTypePropertyAliases
        {
            public const string ProfileFirstName = "firstName";
            public const string ProfileLastName = "lastName";
            public const string ProfilePhoto = "photo";
            public const string ProfileRelatedUser = "relatedUser";
        }
        public class DataTypeTabAliases
        {
            public const string ProfileTabAlias = "Profile";
        }

        public class MemberGroups
        {
            public const string GroupUiPublisher = "UiPublisher";
            public const string GroupWebMaster = "WebMaster";
            public const string GroupUiUser = "UiUser";
        }
    }
}
