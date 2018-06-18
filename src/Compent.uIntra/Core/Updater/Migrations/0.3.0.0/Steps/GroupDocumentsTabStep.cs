using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.GroupsInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using System.Reflection;
using Uintra.Core.Utils;
using Uintra.Navigation;
using Uintra.Core;
using Compent.Uintra.Core.Constants;
using Umbraco.Web;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class GroupDocumentsTabStep : IMigrationStep
    {
        private UmbracoHelper _umbracoHelper;
        private IContentService _contentService;

        public GroupDocumentsTabStep(UmbracoHelper umbracoHelper, IContentService contentService)
        {
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
        }

        public ExecutionResult Execute()
        {
            CreateGroupsDocumentsPageDocType();
            InheritNavigationComposition();
            CreateGroupsDocumentsPageContent();
            return Success;
        }


        private void CreateGroupsDocumentsPageDocType()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.GroupsDocumentsPage,
                Alias = DocumentTypeAliases.GroupsDocumentsPage,
                Icon = DocumentTypeIcons.GroupsDocumentsPage,
                ParentAlias = DocumentTypeAliases.GroupsRoomPage
            };

            InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);
        }

        private void InheritNavigationComposition()
        {
            var nav = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;
            
            InstallationStepsHelper.InheritCompositionForPage(DocumentTypeAliases.GroupsDocumentsPage, nav);
        }

        private void CreateGroupsDocumentsPageContent()
        {
            var groupsRoomPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage));
            if (groupsRoomPage.Children.Any(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsDocumentsPage)))
            {
                return;
            }

            var content = _contentService.CreateContent("Group Documents", groupsRoomPage.Id, DocumentTypeAliasConstants.GroupsDocumentsPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "Group Documents");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, true);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            var gridContent = EmbeddedResourcesUtils.ReadResourceContent($"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.ContentPageJsons.groupsDocumentsPageGrid.json");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}