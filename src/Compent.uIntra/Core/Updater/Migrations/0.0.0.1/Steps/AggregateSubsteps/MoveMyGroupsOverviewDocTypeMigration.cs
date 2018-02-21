using System.Web;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Navigation;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
{
    public class MoveMyGroupsOverviewDocTypeMigration
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly UmbracoContentMigration _contentMigration;

        public MoveMyGroupsOverviewDocTypeMigration()
        {
            _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            _contentService = ApplicationContext.Current.Services.ContentService;
            _umbracoHelper = HttpContext.Current.GetService<UmbracoHelper>();
            _contentMigration = new UmbracoContentMigration();
        }

        public void Execute()
        {
            DeleteContentTypeIfExists();
            CreateContentType();
            CreatePage();
        }

        private void DeleteContentTypeIfExists()
        {
            var myGroupsContentType = _contentTypeService.GetContentType(GroupsInstallationConstants.DocumentTypeAliases.GroupsMyGroupsOverviewPage);
            if (myGroupsContentType != null)
            {
                _contentTypeService.Delete(myGroupsContentType);
            }
        }

        private void CreateContentType()
        {
            var contentType = InstallationStepsHelper.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePage);

            contentType.Name = GroupsInstallationConstants.DocumentTypeNames.GroupsMyGroupsOverviewPage;
            contentType.Alias = GroupsInstallationConstants.DocumentTypeAliases.GroupsMyGroupsOverviewPage;
            contentType.Icon = GroupsInstallationConstants.DocumentTypeIcons.GroupsMyGroupsOverviewPage;

            contentType.AddPropertyGroup(CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            contentType.AddPropertyType(InstallationStepsHelper.GetGridPropertyType(GroupsInstallationConstants.DataTypeNames.GroupGrid), CoreInstallationConstants.DataTypePropertyGroupNames.Content);
            _contentTypeService.Save(contentType);

            InstallationStepsHelper.InheritCompositionForPage(contentType.Alias, NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition);
            InstallationStepsHelper.AddAllowedChildNode(GroupsInstallationConstants.DocumentTypeAliases.GroupsOverviewPage, contentType.Alias);
        }

        private void CreatePage()
        {
            var groupsOverviewPage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(DocumentTypeAliasConstants.HomePage, DocumentTypeAliasConstants.GroupsOverviewPage));

            var content = _contentService.CreateContent("My Groups", groupsOverviewPage.Id, DocumentTypeAliasConstants.GroupsMyGroupsOverviewPage);
            content.SetValue(NavigationPropertiesConstants.NavigationNamePropName, "My Groups");
            content.SetValue(NavigationPropertiesConstants.IsHideFromLeftNavigationPropName, false);
            content.SetValue(NavigationPropertiesConstants.IsHideFromSubNavigationPropName, false);

            _contentMigration.SetGridValueAndSaveAndPublishContent(content, "groupsMyGroupsOverviewPageGrid.json");
        }
    }
}