using System.Reflection;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Uintra.Core.Constants;
using Umbraco.Core;
using Umbraco.Core.Models;
using static Compent.Uintra.Core.Updater.ExecutionResult;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.GroupsInstallationConstants;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class GroupsInstallationStep : IMigrationStep
    {

        public ExecutionResult Execute()
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
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void CreateGroupGridDataType()
        {
            var embeddedResourceFileName = $"{ Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.GroupGridPreValues.json";
            InstallationStepsHelper.CreateGrid(DataTypeNames.GroupGrid, embeddedResourceFileName);
        }

        private void CreateGroupsOverviewPage()
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;

            var page = contentService.GetContentType(DocumentTypeAliases.GroupsOverviewPage);
            if (page != null) return;

            page = InstallationStepsHelper.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePage);

            page.Name = DocumentTypeNames.GroupsOverviewPage;
            page.Alias = DocumentTypeAliases.GroupsOverviewPage;
            page.Icon = DocumentTypeIcons.GroupsOverviewPage;

            page.AddPropertyGroup(CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            page.AddPropertyType(InstallationStepsHelper.GetGridPropertyType(DataTypeNames.GroupGrid), CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            contentService.Save(page);

            InstallationStepsHelper.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, page.Alias);
        }

        private void CreateGroupsRoomPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsRoomPage,
                Alias = DocumentTypeAliases.GroupsRoomPage,
                Icon = DocumentTypeIcons.GroupsRoomPage,
                ParentAlias = DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateMyGroupsOverviewPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsMyGroupsOverviewPage,
                Alias = DocumentTypeAliases.GroupsMyGroupsOverviewPage,
                Icon = DocumentTypeIcons.GroupsMyGroupsOverviewPage,
                ParentAlias = DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsCreatePage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsCreatePage,
                Alias = DocumentTypeAliases.GroupsCreatePage,
                Icon = DocumentTypeIcons.GroupsCreatePage,
                ParentAlias = DocumentTypeAliases.GroupsOverviewPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsEditPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsEditPage,
                Alias = DocumentTypeAliases.GroupsEditPage,
                Icon = DocumentTypeIcons.GroupsEditPage,
                ParentAlias = DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        //private void CreateGroupsDocumentsPage()
        //{
        //    var createModel = new BasePageWithDefaultGridCreateModel
        //    {
        //        Name = DocumentTypeNames.GroupsDocumentsPage,
        //        Alias = DocumentTypeAliases.GroupsDocumentsPage,
        //        Icon = DocumentTypeIcons.GroupsDocumentsPage,
        //        ParentAlias = DocumentTypeAliases.GroupsRoomPage
        //    };

        //    InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        //}

        private void CreateGroupsMembersPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsMembersPage,
                Alias = DocumentTypeAliases.GroupsMembersPage,
                Icon = DocumentTypeIcons.GroupsMembersPage,
                ParentAlias = DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void CreateGroupsDeactivatedGroupPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsDeactivatedGroupPage,
                Alias = DocumentTypeAliases.GroupsDeactivatedGroupPage,
                Icon = DocumentTypeIcons.GroupsDeactivatedGroupPage,
                ParentAlias = DocumentTypeAliases.GroupsRoomPage
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
