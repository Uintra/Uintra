using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Users.Installer
{
    public class UsersInstallationStep: IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Users";
        public int Priority => 2;

        public void Execute()
        {
            CreateProfilePage();
            CreateUsersEditProfilePage();

            CreateMemberUserPickerDataType();
            AddProfileTabProperties();
        }

        private void CreateProfilePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var usersProfilePage = contentService.GetContentType(UsersInstallationConstants.DocumentTypeAliases.ProfilePage);
            if (usersProfilePage != null) return;

            usersProfilePage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            usersProfilePage.Name = UsersInstallationConstants.DocumentTypeNames.ProfilePage;
            usersProfilePage.Alias = UsersInstallationConstants.DocumentTypeAliases.ProfilePage;
            usersProfilePage.Icon = UsersInstallationConstants.DocumentTypeIcons.ProfilePage;

            contentService.Save(usersProfilePage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, UsersInstallationConstants.DocumentTypeAliases.ProfilePage);
        }

        private void CreateUsersEditProfilePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var usersProfileEditPage = contentService.GetContentType(UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage);
            if (usersProfileEditPage != null) return;

            usersProfileEditPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            usersProfileEditPage.Name = UsersInstallationConstants.DocumentTypeNames.ProfileEditPage;
            usersProfileEditPage.Alias = UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage;
            usersProfileEditPage.Icon = UsersInstallationConstants.DocumentTypeIcons.ProfileEditPage;
            
            contentService.Save(usersProfileEditPage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage);
        }

        private void CreateMemberUserPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataType = dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberUserPicker);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(UsersInstallationConstants.DataTypePropertyEditors.MemberUserPicker)
            {
                Name = UsersInstallationConstants.DataTypeNames.MemberUserPicker
            };

            dataTypeService.Save(dataType);
        }

        public static void AddProfileTabProperties()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberType = memberTypeService.Get(UsersInstallationConstants.DataTypeAliases.Member);

            if (!memberType.PropertyGroups.Contains(UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias))
            {
                memberType.AddPropertyGroup(UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias);
            }

            var firstNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.ProfileFirstName,
                Name = UsersInstallationConstants.DataTypePropertyNames.ProfileFirstName,
                Mandatory = true
            };

            var lastNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.ProfileLastName,
                Name = UsersInstallationConstants.DataTypePropertyNames.ProfileLastName,
                Mandatory = true
            };
            var photoProperty = new PropertyType("Umbraco.MediaPicker2", DataTypeDatabaseType.Nvarchar)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.ProfilePhoto,
                Name = UsersInstallationConstants.DataTypePropertyNames.ProfilePhoto,
            };

            var relatedUserDataType = dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberUserPicker);

            var relatedUserProperty = new PropertyType(relatedUserDataType)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.ProfileRelatedUser,
                Name = UsersInstallationConstants.DataTypePropertyNames.ProfileRelatedUser,
            };

            if (!memberType.PropertyTypeExists(UsersInstallationConstants.DataTypePropertyAliases.ProfileFirstName))
            {
                memberType.AddPropertyType(firstNameProperty, UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias);
            }
            if (!memberType.PropertyTypeExists(UsersInstallationConstants.DataTypePropertyAliases.ProfileLastName))
            {
                memberType.AddPropertyType(lastNameProperty, UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias);
            }
            if (!memberType.PropertyTypeExists(UsersInstallationConstants.DataTypePropertyAliases.ProfilePhoto))
            {
                memberType.AddPropertyType(photoProperty, UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias);
            }
            if (!memberType.PropertyTypeExists(UsersInstallationConstants.DataTypePropertyAliases.ProfileRelatedUser))
            {
                memberType.AddPropertyType(relatedUserProperty, UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias);
            }

            memberTypeService.Save(memberType);
        }
    }
}
