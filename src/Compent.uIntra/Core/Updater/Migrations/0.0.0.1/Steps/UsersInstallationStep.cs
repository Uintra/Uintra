using Umbraco.Core;
using Umbraco.Core.Models;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.UsersInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class UsersInstallationStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            CreateProfilePage();
            CreateUsersEditProfilePage();

            CreateMemberUserPickerDataType();
            CreateMemberChangesWatcherDataType();
            AddMembershipTabProperties();
            AddProfileTabProperties();
            AddDefaultMemberGroups();
            AddDefaultMember();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateProfilePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.ProfilePage,
                Alias = DocumentTypeAliases.ProfilePage,
                Icon = DocumentTypeIcons.ProfilePage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateUsersEditProfilePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.ProfileEditPage,
                Alias = DocumentTypeAliases.ProfileEditPage,
                Icon = DocumentTypeIcons.ProfileEditPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateMemberUserPickerDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataType =
                dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.MemberUserPicker);
            if (dataType != null) return;

            dataType = new DataTypeDefinition(DataTypePropertyEditors.MemberUserPicker)
            {
                Name = DataTypeNames.MemberUserPicker,
                DatabaseType = DataTypeDatabaseType.Nvarchar
            };

            dataTypeService.Save(dataType);
        }

        private void CreateMemberChangesWatcherDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var dataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.MemberChangesWatcher);
            if (dataType != null)
            {
                return;
            }

            dataType = new DataTypeDefinition(DataTypePropertyEditors.MemberChangesWatcher)
            {
                Name = DataTypeNames.MemberChangesWatcher,
                DatabaseType = DataTypeDatabaseType.Nvarchar
            };

            dataTypeService.Save(dataType);
        }

        private static void AddMembershipTabProperties()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberType = memberTypeService.Get(DataTypeAliases.Member);

            var memberChangesWatcherDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.MemberChangesWatcher);

            var memberChangesWatcherProperty = new PropertyType(memberChangesWatcherDataType)
            {
                Alias = DataTypePropertyAliases.MembershipMemberChangesWatcher,
                Name = DataTypePropertyNames.MembershipMemberChangesWatcher,
            };

            if (!memberType.PropertyTypeExists(memberChangesWatcherProperty.Alias))
            {
                memberType.AddPropertyType(memberChangesWatcherProperty, DataTypeTabAliases.MembershipTabAlias);
            }

            memberTypeService.Save(memberType);
        }

        private static void AddProfileTabProperties()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberType = memberTypeService.Get(DataTypeAliases.Member);

            if (!memberType.PropertyGroups.Contains(DataTypeTabAliases.ProfileTabAlias))
            {
                memberType.AddPropertyGroup(DataTypeTabAliases.ProfileTabAlias);
            }

            var firstNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = DataTypePropertyAliases.ProfileFirstName,
                Name = DataTypePropertyNames.ProfileFirstName,
                Mandatory = true
            };

            var lastNameProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = DataTypePropertyAliases.ProfileLastName,
                Name = DataTypePropertyNames.ProfileLastName,
                Mandatory = true
            };
            var photoProperty = new PropertyType("Umbraco.MediaPicker2", DataTypeDatabaseType.Nvarchar)
            {
                Alias = DataTypePropertyAliases.ProfilePhoto,
                Name = DataTypePropertyNames.ProfilePhoto,
            };

            var relatedUserDataType =
                dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.MemberUserPicker);

            var relatedUserProperty = new PropertyType(relatedUserDataType)
            {
                Alias = DataTypePropertyAliases.ProfileRelatedUser,
                Name = DataTypePropertyNames.ProfileRelatedUser,
            };

            var profileTab = DataTypeTabAliases.ProfileTabAlias;

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

            var uiUserGroup = memberGroupService.GetByName(MemberGroups.GroupUiUser);
            var webMasterGroup = memberGroupService.GetByName(MemberGroups.GroupWebMaster);
            var uiPublisherGroup = memberGroupService.GetByName(MemberGroups.GroupUiPublisher);

            if (uiUserGroup == null)
            {
                uiUserGroup = new MemberGroup
                {
                    Name = MemberGroups.GroupUiUser,
                    CreatorId = 0
                };
                memberGroupService.Save(uiUserGroup);
            }
            if (webMasterGroup == null)
            {
                webMasterGroup = new MemberGroup
                {
                    Name = MemberGroups.GroupWebMaster,
                    CreatorId = 0
                };
                memberGroupService.Save(webMasterGroup);
            }
            if (uiPublisherGroup == null)
            {
                uiPublisherGroup = new MemberGroup
                {
                    Name = MemberGroups.GroupUiPublisher,
                    CreatorId = 0
                };
                memberGroupService.Save(uiPublisherGroup);
            }
        }

        private static void AddDefaultMember()
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var member = memberService.GetByEmail(DefaultMember.Email);
            if (member != null)
            {
                return;
            }

            member = memberService.CreateMember(DefaultMember.Name, DefaultMember.Email, DefaultMember.Name, DataTypeAliases.Member);
            member.SetValue(DataTypePropertyAliases.ProfileFirstName, DefaultMember.Name);
            member.SetValue(DataTypePropertyAliases.ProfileLastName, DefaultMember.Name);
            member.SetValue(DataTypePropertyAliases.ProfileRelatedUser, DefaultMember.UmbracoAdminUserId);

            memberService.Save(member, raiseEvents: false);
        }
    }
}
