using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using Uintra.Core.Constants;
using Uintra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.NotificationInstallationConstants;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps
{
    public class NotificationInstallationStep : IMigrationStep
    {

        public ExecutionResult Execute()
        {
            CreateNotificationTypeEnumDataType();
            CreateNotificationPage();
            return Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }


        private void CreateNotificationTypeEnumDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.NotificationTypeEnum);
            if (folderTypeDataType != null) return;

            folderTypeDataType = new DataTypeDefinition(-1, UmbracoAliases.EnumDropdownList)
            {
                Name = DataTypeNames.NotificationTypeEnum
            };

            var preValues = new Dictionary<string, PreValue>
            {
                { DataTypePropertyAliases.Assembly, new PreValue(DataTypePropertyPreValues.Assembly)},
                { DataTypePropertyAliases.Enum, new PreValue(typeof(NotificationTypeEnum).FullName)}
            };
            dataTypeService.SaveDataTypeAndPreValues(folderTypeDataType, preValues);
        }

        private void CreateNotificationPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = DocumentTypeNames.NotificationPage,
                Alias = DocumentTypeAliases.NotificationPage,
                Icon = DocumentTypeIcons.NotificationPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            var notificationPage = InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

            var itemCountForPopupProperty = new PropertyType("Umbraco.Integer", DataTypeDatabaseType.Integer)
            {
                Name = DocumentTypePropertyNames.ItemCountForPopup,
                Alias = DocumentTypePropertyAliases.ItemCountForPopup
            };
            notificationPage.AddPropertyType(itemCountForPopupProperty, "Content");

            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            contentService.Save(notificationPage);
        }
    }
}
