using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._3._0._0;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._1._0.Steps
{
    public class UpdateContentGridEditors : IMigrationStep
    {
        private const string TableEditor = "TableEditor";

        private readonly IDataTypeService _dataTypeService;

        public UpdateContentGridEditors(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        public ExecutionResult Execute()
        {
            AddToDefaultGridAllowedEditors();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            
        }

        private void AddToDefaultGridAllowedEditors()
        {
            var contentGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ContentGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(contentGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.AddAllowedEditorForOneColumnRow(preValuesDictionary, TableEditor);
            GridInstallationHelper.AddAllowedEditorForTwoColumnRow(preValuesDictionary, TableEditor);

            _dataTypeService.SaveDataTypeAndPreValues(contentGridDataType, preValuesDictionary);
        }
    }
}