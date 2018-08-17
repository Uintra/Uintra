using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._3._0._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._3._0.Steps
{
    public class UserListStep : IMigrationStep
    {
        private const string UserListAlias = "custom.UserList";

        private readonly IDataTypeService _dataTypeService;

        public UserListStep(IDataTypeService dataTypeService)
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
            RemoveFromDefaultGridAllowedEditors();
        }

        private void AddToDefaultGridAllowedEditors()
        {
            var defaultGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ContentGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(defaultGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.AddAllowedEditorForOneColumnRow(preValuesDictionary, UserListAlias);

            _dataTypeService.SavePreValues(defaultGridDataType, preValuesDictionary);
        }

        private void RemoveFromDefaultGridAllowedEditors()
        {
            var defaultGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.DefaultGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(defaultGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.RemoveAllowedEditorForOneColumnRow(preValuesDictionary, UserListAlias);

            _dataTypeService.SavePreValues(defaultGridDataType, preValuesDictionary);
        }
    }
}