using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._1._2.Steps
{
    public class UseInSearchDefaultTrueStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            return SetDefaultValueForUseInSearchDataType("1");
        }

        public void Undo()
        {
            SetDefaultValueForUseInSearchDataType("0");
        }

        private static ExecutionResult SetDefaultValueForUseInSearchDataType(string value)
        {
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var contentPage = contentService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.ContentPage);
            if (contentPage == null) return ExecutionResult.Failure(new Exception("ContentPage is absent"));

            if (!contentPage.PropertyTypeExists(SearchInstallationConstants.DocumentTypePropertyAliases.UseInSearch))
            {
                return ExecutionResult.Failure(new Exception("UseInSearch property is absent"));
            }

            var useInSearchDataType = dataTypeService.GetDataTypeDefinitionByName(SearchInstallationConstants.DataTypeNames.ContentPageUseInSearch);
            var preValues = new Dictionary<string, PreValue> { { "default", new PreValue(value) } };
            dataTypeService.SaveDataTypeAndPreValues(useInSearchDataType, preValues);

            return ExecutionResult.Success;
        }
    }
}