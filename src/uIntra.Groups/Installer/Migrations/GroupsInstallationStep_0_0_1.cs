using System.Reflection;
using uIntra.Core.Constants;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Groups.Installer.Migrations
{
    public class GroupsInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Groups";
        public int Priority => 2;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateGroupGridDataType();
            CreateGroupsOverviewPage();
            CreateGroupsRoomPage();
            CreateMyGroupsOverviewPage();
            CreateGroupsCreatePage();
            CreateGroupsEditPage();
            //CreateGroupsDocumentsPage(); This functionality under construction now.
            CreateGroupsMembersPage();
            CreateGroupsDeactivatedGroupPage();

            AddFolderGroupIdProperty();
        }

        private void CreateGroupGridDataType()
        {
            var embeddedResourceFileName = "uIntra.Groups.Installer.PreValues.GroupGridPreValues.json";
            InstallationStepsHelper.CreateGrid(GroupsInstallationConstants.DataTypeNames.GroupGrid, embeddedResourceFileName);
        }

        private void CreateGroupsOverviewPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage);
            if (page != null) return;

            page = InstallationStepsHelper.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePage);

            page.Name = GroupsInstallationConstants.DocumentTypeNames.GroupsOverviewPage;
            page.Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage;
            page.Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsOverviewPage;

            page.AddPropertyGroup(CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            page.AddPropertyType(InstallationStepsHelper.GetGridPropertyType(GroupsInstallationConstants.DataTypeNames.GroupGrid), CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            contentService.Save(page);

            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, page.Alias);
        }

        private void CreateGroupsRoomPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsRoomPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsRoomPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateMyGroupsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsMyGroupsOverviewPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsMyGroupsOverviewPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsMyGroupsOverviewPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsCreatePage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsCreatePage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsCreatePage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsEditPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsEditPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsEditPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsDocumentsPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsDocumentsPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsDocumentsPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsDocumentsPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsMembersPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsMembersPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsMembersPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsMembersPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsDeactivatedGroupPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = GroupsInstallationConstants.DocumentTypeNames.GroupsDeactivatedGroupPage,
                Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsDeactivatedGroupPage,
                Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsDeactivatedGroupPage,
                ParentAlias = GroupsInstallationConstants.DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        public static void AddFolderGroupIdProperty()
        {
            var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

            var folderType = contentTypeService.GetMediaType(UmbracoAliases.Media.FolderTypeAlias);

            var groupIdProperty = new PropertyType("Umbraco.NoEdit", DataTypeDatabaseType.Nvarchar, "GroupId")
            {
                Name = "Group Id"
            };

            if (!folderType.PropertyTypeExists(groupIdProperty.Alias))
            {
                folderType.AddPropertyType(groupIdProperty);
                contentTypeService.Save(folderType);
            }
        }
    }
}
