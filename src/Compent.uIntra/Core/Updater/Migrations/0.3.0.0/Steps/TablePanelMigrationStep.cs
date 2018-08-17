using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class TablePanelMigrationStep : IMigrationStep
    {
        private const string TableEditor = "TableEditor";
        private const string AllowedEditorsSection = "allowedEditors";

        private readonly IDataTypeService _dataTypeService;

        public TablePanelMigrationStep(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        public ExecutionResult Execute()
        {
            AddToPanelPickerAllowedEditors();
            AddToContentGridAllowedEditors();

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            RemoveFromPanelPickerAllowedEditors();
            RemoveFromContentGridAllowedEditors();
        }

        private void AddToPanelPickerAllowedEditors()
        {
            var panelPickerDataType = _dataTypeService.GetDataTypeDefinitionByName(PanelsInstallationConstants.DataTypeNames.PanelPicker);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(panelPickerDataType.Id).PreValuesAsDictionary;

            AddAllowedEditor(preValuesDictionary, TableEditor);
            _dataTypeService.SavePreValues(panelPickerDataType, preValuesDictionary);
        }

        private void RemoveFromPanelPickerAllowedEditors()
        {
            var panelPickerDataType = _dataTypeService.GetDataTypeDefinitionByName(PanelsInstallationConstants.DataTypeNames.PanelPicker);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(panelPickerDataType.Id).PreValuesAsDictionary;

            DeleteAllowedEditor(preValuesDictionary, TableEditor);
            _dataTypeService.SavePreValues(panelPickerDataType, preValuesDictionary);
        }

        private void AddAllowedEditor(IDictionary<string, PreValue> preValuesDictionary, string allowedEditorAlias)
        {
            foreach (var preValueItem in preValuesDictionary)
            {
                var parsedPreValue = JObject.Parse(preValueItem.Value.Value);
                var allowedEditorsToken = parsedPreValue.SelectToken(AllowedEditorsSection);
                if (allowedEditorsToken != null)
                {
                    var allowedEditorAliasToken = allowedEditorsToken.SelectToken($"[?(@ == '{allowedEditorAlias}')]");
                    if (allowedEditorAliasToken != null) return;

                    allowedEditorsToken.Last.AddAfterSelf(allowedEditorAlias);
                    preValueItem.Value.Value = parsedPreValue.ToString();
                }
            }
        }

        private void DeleteAllowedEditor(IDictionary<string, PreValue> preValuesDictionary, string allowedEditorAlias)
        {
            foreach (var preValueItem in preValuesDictionary)
            {
                var parsedPreValue = JObject.Parse(preValueItem.Value.Value);
                var allowedEditorAliasToken = parsedPreValue.SelectToken($"{AllowedEditorsSection}[?(@ == '{allowedEditorAlias}')]");
                if (allowedEditorAliasToken != null)
                {
                    allowedEditorAliasToken.Remove();
                    preValueItem.Value.Value = parsedPreValue.ToString();
                    return;
                }
            }
        }

        private void AddToContentGridAllowedEditors()
        {
            var contentGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ContentGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(contentGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.AddAllowedEditorForOneColumnRow(preValuesDictionary, TableEditor);
            GridInstallationHelper.AddAllowedEditorForTwoColumnRow(preValuesDictionary, TableEditor);

            _dataTypeService.SavePreValues(contentGridDataType, preValuesDictionary);
        }

        private void RemoveFromContentGridAllowedEditors()
        {
            var contentGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ContentGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(contentGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.RemoveAllowedEditorForOneColumnRow(preValuesDictionary, TableEditor);
            GridInstallationHelper.RemoveAllowedEditorForTwoColumnRow(preValuesDictionary, TableEditor);

            _dataTypeService.SavePreValues(contentGridDataType, preValuesDictionary);
        }
    }
}