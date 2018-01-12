using uIntra.Core.Installer;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Users.Installers.Migrations
{
    public class UsersInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Users";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateProfilePage();
            CreateUsersEditProfilePage();

            CreateMemberUserPickerDataType();
            CreateMemberChangesWatcherDataType();
            AddMembershipTabProperties();
            AddProfileTabProperties();
            AddDefaultMemberGroups();
            AddDefaultMember();
        }

        private void CreateProfilePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = UsersInstallationConstants.DocumentTypeNames.ProfilePage,
                Alias = UsersInstallationConstants.DocumentTypeAliases.ProfilePage,
                Icon = UsersInstallationConstants.DocumentTypeIcons.ProfilePage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateUsersEditProfilePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = UsersInstallationConstants.DocumentTypeNames.ProfileEditPage,
                Alias = UsersInstallationConstants.DocumentTypeAliases.ProfileEditPage,
                Icon = UsersInstallationConstants.DocumentTypeIcons.ProfileEditPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
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
                DatabaseType = DataTypeDatabaseType.Nvarchar
            };

            dataTypeService.Save(dataType);
        }

        private void CreateMemberChangesWatcherDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataType = dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberChangesWatcher);
            if (dataType != null)
            {
                return;
            }

            dataType = new DataTypeDefinition(UsersInstallationConstants.DataTypePropertyEditors.MemberChangesWatcher)
            {
                Name = UsersInstallationConstants.DataTypeNames.MemberChangesWatcher,
                DatabaseType = DataTypeDatabaseType.Nvarchar
            };

            dataTypeService.Save(dataType);
        }

        private static void AddMembershipTabProperties()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberType = memberTypeService.Get(UsersInstallationConstants.DataTypeAliases.Member);

            var memberChangesWatcherDataType = dataTypeService.GetDataTypeDefinitionByName(UsersInstallationConstants.DataTypeNames.MemberChangesWatcher);

            var memberChangesWatcherProperty = new PropertyType(memberChangesWatcherDataType)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.MembershipMemberChangesWatcher,
                Name = UsersInstallationConstants.DataTypePropertyNames.MembershipMemberChangesWatcher,
            };

            if (!memberType.PropertyTypeExists(memberChangesWatcherProperty.Alias))
            {
                memberType.AddPropertyType(memberChangesWatcherProperty, UsersInstallationConstants.DataTypeTabAliases.MembershipTabAlias);
            }

            memberTypeService.Save(memberType);
        }

        private static void AddProfileTabProperties()
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

        private static void AddDefaultMemberGroups()
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

        private static void AddDefaultMember()
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
