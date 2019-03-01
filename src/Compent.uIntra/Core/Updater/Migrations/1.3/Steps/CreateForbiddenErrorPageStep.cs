using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Uintra.Core.Extensions;
using Uintra.Core.Utils;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._1._3.Steps
{
    public class CreateForbiddenErrorPageStep : IMigrationStep
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;
        private const string ForbiddenPageName = "Forbidden";

        public CreateForbiddenErrorPageStep()
        {
            _umbracoHelper = HttpContext.Current.GetService<UmbracoHelper>();
            _contentService = ApplicationContext.Current.Services.ContentService;
        }
        public ExecutionResult Execute()
        {
            CreateForbiddenErrorPage();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private void CreateForbiddenErrorPage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));
            var errorPages = homePage.Children.Where(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.ErrorPage));

            if (errorPages.Any(ep=>ep.Name== ForbiddenPageName))
            {
                return;
            }

            var content = _contentService.CreateContent(ForbiddenPageName, homePage.Id, DocumentTypeAliasConstants.ErrorPage);

            SetGridValueAndSaveAndPublishContent(content, "forbiddenPageGrid.json");            
        }

        public void SetGridValueAndSaveAndPublishContent(IContent content, string gridEmbeddedResourceFileName)
        {
            var gridContent = EmbeddedResourcesUtils.ReadResourceContent($"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._1._3.PreValues.{gridEmbeddedResourceFileName}");
            content.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(content);
        }
    }
}