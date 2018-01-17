using System.Collections.Generic;
using uIntra.Core.Constants;
using uIntra.Core.Installer;
using uIntra.Notification.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace uIntra.Notification.Installer.Migrations
{
    public class NotificationInstallationStep_0_0_1 : IIntranetInstallationStep
    {
        public string PackageName => "uIntra.Notification";
        public int Priority => 0;
        public string Version => InstallationVersionConstrants.Version_0_0_1;

        public void Execute()
        {
            CreateNotificationTypeEnumDataType();
            CreateNotificationPage();
        }
        private void CreateNotificationTypeEnumDataType()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;

            var folderTypeDataType = dataTypeService.GetDataTypeDefinitionByName(NotificationInstallationConstants.DataTypeNames.NotificationTypeEnum);
            if (folderTypeDataType != null) return;

            folderTypeDataType = new DataTypeDefinition(-1, UmbracoAliases.EnumDropdownList)
            {
                Name = NotificationInstallationConstants.DataTypeNames.NotificationTypeEnum
            };

            var preValues = new Dictionary<string, PreValue>
            {
                { NotificationInstallationConstants.DataTypePropertyAliases.Assembly, new PreValue(NotificationInstallationConstants.DataTypePropertyPreValues.Assembly)},
                { NotificationInstallationConstants.DataTypePropertyAliases.Enum, new PreValue(typeof(NotificationTypeEnum).FullName)}
            };
            dataTypeService.SaveDataTypeAndPreValues(folderTypeDataType, preValues);
        }

        private void CreateNotificationPage()
        {
            var createModel = new BasePageWithDefaultGridCreateModel
            {
                Name = NotificationInstallationConstants.DocumentTypeNames.NotificationPage,
                Alias = NotificationInstallationConstants.DocumentTypeAliases.NotificationPage,
                Icon = NotificationInstallationConstants.DocumentTypeIcons.NotificationPage,
                ParentAlias = CoreInstallationConstants.DocumentTypeAliases.HomePage
            };

            var notificationPage = InstallationStepsHelper.CreatePageDocTypeWithBaseGrid(createModel);

            var itemCountForPopupProperty = new PropertyType("Umbraco.Integer", DataTypeDatabaseType.Integer)
            {
                Name = NotificationInstallationConstants.DocumentTypePropertyNames.ItemCountForPopup,
                Alias = NotificationInstallationConstants.DocumentTypePropertyAliases.ItemCountForPopup
            };
            notificationPage.AddPropertyType(itemCountForPopupProperty, "Content");

            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            contentService.Save(notificationPage);
        }
    }
}
