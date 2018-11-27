using Uintra.Core.Media;
using Uintra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Services;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.NotificationInstallationConstants;

namespace Compent.Uintra.Core.Updater.Migrations._1._0.Steps
{
    public class UpdateEnumDropDownListPreValuesStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            UpdateDatatypePreValues(dataTypeService, FolderConstants.DataTypeName, 
                FolderConstants.EnumAssemblyDll, typeof(MediaFolderTypeEnum).FullName);
            UpdateDatatypePreValues(dataTypeService, DataTypeNames.NotificationTypeEnum,
                DataTypePropertyPreValues.Assembly, typeof(NotificationTypeEnum).FullName); //NotificationInstallationConstants.DataTypePropertyPreValues.Enum

            return ExecutionResult.Success;
        }

        public void Undo()
        {
        }
        private void UpdateDatatypePreValues(IDataTypeService dataTypeService, string dataTypeName,
            string assemblyFullNamePreValue, string enumFullNamePreValue)
        {
            var dataType = dataTypeService.GetDataTypeDefinitionByName(dataTypeName);
            if (dataType != null)
            {
                var preValues = dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id);
                var dictionary = preValues.FormatAsDictionary();
                dictionary[FolderConstants.PreValueAssemblyAlias].Value = assemblyFullNamePreValue;
                dictionary[FolderConstants.PreValueEnumAlias].Value = enumFullNamePreValue;

                dataTypeService.SaveDataTypeAndPreValues(dataType, dictionary);
            }
        }
    }
}