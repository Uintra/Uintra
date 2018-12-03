using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Compent.Extensions;
using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Grid;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._2._0.Steps
{
    public class HomePageHideUnnecessaryItemsStep : IMigrationStep
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;
        private readonly IDataTypeService _dataTypeService;

        public HomePageHideUnnecessaryItemsStep(
            UmbracoHelper umbracoHelper,
            IContentService contentService,
            IDataTypeService dataTypeService
        )
        {
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
            _dataTypeService = dataTypeService;
        }

        public ExecutionResult Execute()
        {
            var contentToRemove = new List<string>
            {
                "custom.NewsEdit",
                "custom.NewsCreate",
                "custom.NewsDetails",
                "custom.EventsCreate",
                "custom.EventsEdit",
                "custom.EventsDetails"
            };

            var homePage = _umbracoHelper
                .TypedContentAtRoot()
                .FirstOrDefault(el => el.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.HomePage));

            if (homePage == null)
            {
                return ExecutionResult.Failure(new Exception("Home page is null"));
            }

            RemoveFromContent(contentToRemove, homePage);
            RemoveFromPreValues(contentToRemove);

            return ExecutionResult.Success;
        }

        private void RemoveFromContent(IEnumerable<string> editorsToRemove, IPublishedContent homePage)
        {
            var grid = homePage.GetPropertyValue<dynamic>(UmbracoContentMigrationConstants.Grid.GridPropName);
            List<string> allowedContents = grid.sections[0].rows[0].areas[0].allowed.ToObject<List<string>>();
            allowedContents.RemoveAll(editorsToRemove.Contains);

            grid.sections[0].rows[0].areas[0].allowed = JArray.FromObject(allowedContents).ToDynamic();
            string tt = grid.ToString();

            var homePageContent = _contentService.GetById(homePage.Id);
            homePageContent.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, tt);
            _contentService.SaveAndPublishWithStatus(homePageContent);
        }

        private void RemoveFromPreValues(IEnumerable<string> editorsToRemove)
        {
            const string itemsKey = "items";

            var defaultGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.DefaultGrid);
            var preValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(defaultGridDataType.Id);
            var preValuesDictionary = preValues.FormatAsDictionary();
            var items = preValuesDictionary[itemsKey].Value.Pipe(Json.Decode);
            var availableEditors = items.layouts[0].areas[0].allowed;

            var availableEditorsList = new List<string>();

            foreach (var editor in availableEditors)
            {
                availableEditorsList.Add((string) editor);
            }

            var actualEditors = availableEditorsList.Except(editorsToRemove);
            items.layouts[0].areas[0].allowed = JArray.FromObject(actualEditors).ToDynamic();
            preValuesDictionary[itemsKey].Value = JsonConvert.SerializeObject(items);

            _dataTypeService.SaveDataTypeAndPreValues(defaultGridDataType, preValuesDictionary);

        }

        public void Undo()
        {

        }
    }
}