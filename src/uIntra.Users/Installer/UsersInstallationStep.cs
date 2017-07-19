using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Users.Installer
{
    public class UsersInstallationStep : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Users";
        public int Priority => 2;

        public void Execute()
        {
            CreateProfilePage();
            CreateUsersEditProfilePage();

            CreateMemberUserPickerDataType();
            AddProfileTabProperties();
            AddDefaultMemberGroups();
            AddDefaultMember();
        }

        private void CreateProfilePage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var usersProfilePage =
                contentService.GetContentType(UsersInstallationConstants.DocumentTypeAliases.ProfilePage);
            if (usersProfilePage != null) return;

            usersProfilePage =
                CoreInstallationStep.GetBasePageWithGridBase(
                    CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
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

            var usersProfileEditPage =
                contentService.GetContentType(UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage);
            if (usersProfileEditPage != null) return;

            usersProfileEditPage = CoreInstallationStep.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithGrid);
            //TODO: Move static methods to service

            usersProfileEditPage.Name = UsersInstallationConstants.DocumentTypeNames.ProfileEditPage;
            usersProfileEditPage.Alias = UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage;
            usersProfileEditPage.Icon = UsersInstallationConstants.DocumentTypeIcons.ProfileEditPage;

            contentService.Save(usersProfileEditPage);
            CoreInstallationStep.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage,
                UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage);
        }

        private void CreateMemberUserPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataType =
                dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberUserPicker);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(UsersInstallationConstants.DataTypePropertyEditors.MemberUserPicker)
            {
                Name = UsersInstallationConstants.DataTypeNames.MemberUserPicker,
                DatabaseType = DataTypeDatabaseType.Integer
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

            var relatedUserDataType =
                dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberUserPicker);

            var relatedUserProperty = new PropertyType(relatedUserDataType)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.ProfileRelatedUser,
                Name = UsersInstallationConstants.DataTypePropertyNames.ProfileRelatedUser,
            };

            var profileTab = UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias;

            if (!memberType.PropertyTypeExists(firstNameProperty.Alias))
            {
                memberType.AddPropertyType(firstNameProperty, profileTab);
            }
            if (!memberType.PropertyTypeExists(lastNameProperty.Alias))
            {
                memberType.AddPropertyType(lastNameProperty, profileTab);
            }
            if (!memberType.PropertyTypeExists(photoProperty.Alias))
            {
                memberType.AddPropertyType(photoProperty, profileTab);
            }
            if (!memberType.PropertyTypeExists(relatedUserProperty.Alias))
            {
                memberType.AddPropertyType(relatedUserProperty, profileTab);
            }

            memberTypeService.Save(memberType);
        }

        public static void AddDefaultMemberGroups()
        {
            var memberGroupService = ApplicationContext.Current.Services.MemberGroupService;

            var uiUserGroup = memberGroupService.GetByName(UsersInstallationConstants.MemberGroups.GroupUiUser);
            var webMasterGroup = memberGroupService.GetByName(UsersInstallationConstants.MemberGroups.GroupWebMaster);
            var uiPublisherGroup = memberGroupService.GetByName(UsersInstallationConstants.MemberGroups.GroupUiPublisher);

            if (uiUserGroup == null)
            {
                uiUserGroup = new MemberGroup
                {
                    Name = UsersInstallationConstants.MemberGroups.GroupUiUser,
                    CreatorId = 0
                };
                memberGroupService.Save(uiUserGroup);
            }
            if (webMasterGroup == null)
            {
                webMasterGroup = new MemberGroup
                {
                    Name = UsersInstallationConstants.MemberGroups.GroupWebMaster,
                    CreatorId = 0
                };
                memberGroupService.Save(webMasterGroup);
            }
            if (uiPublisherGroup == null)
            {
                uiPublisherGroup = new MemberGroup
                {
                    Name = UsersInstallationConstants.MemberGroups.GroupUiPublisher,
                    CreatorId = 0
                };
                memberGroupService.Save(uiPublisherGroup);
            }
        }

        public static void AddDefaultMember()
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var member = memberService.GetByEmail(UsersInstallationConstants.DefaultMember.Email);
            if (member != null)
            {
                return;
            }

            member = memberService.CreateMember(UsersInstallationConstants.DefaultMember.Name, UsersInstallationConstants.DefaultMember.Email, UsersInstallationConstants.DefaultMember.Name,
                UsersInstallationConstants.DataTypeAliases.Member);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileFirstName, UsersInstallationConstants.DefaultMember.Name);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileLastName, UsersInstallationConstants.DefaultMember.Name);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileRelatedUser, UsersInstallationConstants.DefaultMember.UmbracoAdminUserId);

            memberService.Save(member);
            memberService.SavePassword(member, UsersInstallationConstants.DefaultMember.Password);
            memberService.AssignRole(member.Id, UsersInstallationConstants.MemberGroups.GroupWebMaster);
        }
    }
}
