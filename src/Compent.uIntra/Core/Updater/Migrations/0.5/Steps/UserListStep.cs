using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._3._0._0;
using System.Collections.Generic;
using Uintra.Core.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._0._5.Steps
{
    public class UserListStep : IMigrationStep
    {
        private const string UserListAlias = "custom.UserList";
        private const string dataTypeName = "Content page picker";
        private const string propertyName = "User list page";
        private const string propertyAlias = "userListPage";

        private readonly IDataTypeService _dataTypeService;

        public UserListStep(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        public ExecutionResult Execute()
        {
            AddToDefaultGridAllowedEditors();
            AddContentPagePicker();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            RemoveFromDefaultGridAllowedEditors();
            RemoveContentPagePicker();
        }

        private void AddToDefaultGridAllowedEditors()
        {
            var defaultGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.ContentGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(defaultGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.AddAllowedEditorForOneColumnRow(preValuesDictionary, UserListAlias);

            _dataTypeService.SaveDataTypeAndPreValues(defaultGridDataType, preValuesDictionary);
        }

        private void RemoveFromDefaultGridAllowedEditors()
        {
            var defaultGridDataType = _dataTypeService.GetDataTypeDefinitionByName(CoreInstallationConstants.DataTypeNames.DefaultGrid);
            var preValuesDictionary = _dataTypeService.GetPreValuesCollectionByDataTypeId(defaultGridDataType.Id).PreValuesAsDictionary;

            GridInstallationHelper.RemoveAllowedEditorForOneColumnRow(preValuesDictionary, UserListAlias);

            _dataTypeService.SaveDataTypeAndPreValues(defaultGridDataType, preValuesDictionary);
        }

        private void AddContentPagePicker()
        {
            var services = ApplicationContext.Current.Services;
            var dtdService = services.DataTypeService;
            var contentPagePickerDtd = dtdService.GetDataTypeDefinitionByName(dataTypeName);
            if (contentPagePickerDtd == null)
            {
                contentPagePickerDtd = new DataTypeDefinition(-1, "Umbraco.MultiNodeTreePicker2")
                {
                    Name = dataTypeName
                };
                var preValues = new Dictionary<string, PreValue>
                {
                    { "startNode", new PreValue(new { type = "content", query = $"$root/{CoreInstallationConstants.DocumentTypeAliases.HomePage}" }.ToJson())},
                    { "filter", new PreValue(CoreInstallationConstants.DocumentTypeAliases.ContentPage) },
                    { "minNumber", new PreValue("0")},
                    { "maxNumber", new PreValue("1")},
                    { "showOpenButton", new PreValue("0") }
                };
                dtdService.SaveDataTypeAndPreValues(contentPagePickerDtd, preValues);
            }

            var contentTypeService = services.ContentTypeService;
            var page = contentTypeService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.HomePage);
            if (page == null) return;
            if (page.PropertyTypeExists(propertyAlias))
                return;
            var property = new PropertyType(contentPagePickerDtd)
            {
                Name = propertyName,
                Alias = propertyAlias
            };

            page.AddPropertyGroup(NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
            page.AddPropertyType(property, NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
            contentTypeService.Save(page);
        }

        private void RemoveContentPagePicker()
        {
            var services = ApplicationContext.Current.Services;
            var contentTypeService = services.ContentTypeService;
            var page = contentTypeService.GetContentType(CoreInstallationConstants.DocumentTypeAliases.HomePage);
            if (page != null)
            {
                if (page.PropertyTypeExists(propertyAlias))
                {
                    page.RemovePropertyType(propertyAlias);
                    page.RemovePropertyGroup(NavigationInstallationConstants.DocumentTypeTabNames.Navigation);
                    contentTypeService.Save(page);
                }
                var dtdService = services.DataTypeService;
                var dtd = dtdService.GetDataTypeDefinitionByName(dataTypeName);
                if (dtd == null) return;
                dtdService.Delete(dtd);
            }
        }
    }
}