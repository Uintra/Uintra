using System;
using System.Linq;
using System.Reflection;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Uintra.Core;
using Uintra.Core.Utils;
using Uintra.Navigation;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Compent.Uintra.Core.Updater.ExecutionResult;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.GroupsInstallationConstants;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class GroupDocumentsTabStep : IMigrationStep
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;

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
            ResortGroupTabs();
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

        private void ResortGroupTabs()
        {
            var groupsRoomPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage,
                DocumentTypeAliasConstants.GroupsOverviewPage, DocumentTypeAliasConstants.GroupsRoomPage));

            var sortedContentIds = groupsRoomPage.Children
                .Select(c => (
                    rank: c.DocumentTypeAlias == DocumentTypeAliasConstants.GroupsDocumentsPage ? 0 : 1,
                    contentId: _contentService.GetById(c.Id)))
                .OrderBy(ordered => ordered.rank)
                .Select(ordered => ordered.contentId);

            _contentService.Sort(sortedContentIds);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}