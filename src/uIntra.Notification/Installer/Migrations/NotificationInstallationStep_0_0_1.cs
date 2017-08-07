using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Installer;
using uIntra.Core.Installer.Migrations;
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
            var contentService = ApplicationContext.Current.Services.ContentTypeService;
            var notificationPage = contentService.GetContentType(NotificationInstallationConstants.DocumentTypeAliases.NotificationPage);
            if (notificationPage != null) return;

            notificationPage = CoreInstallationStep_0_0_1.GetBasePageWithGridBase(CoreInstallationConstants.DocumentTypeAliases.BasePageWithContentGrid);

            notificationPage.Name = NotificationInstallationConstants.DocumentTypeNames.NotificationPage;
            notificationPage.Alias = NotificationInstallationConstants.DocumentTypeAliases.NotificationPage;
            notificationPage.Icon = NotificationInstallationConstants.DocumentTypeIcons.NotificationPage;

            var itemCountForPopupProperty = new PropertyType("Umbraco.Integer", DataTypeDatabaseType.Integer)
            {
                Name = NotificationInstallationConstants.DocumentTypePropertyNames.ItemCountForPopup,
                Alias = NotificationInstallationConstants.DocumentTypePropertyAliases.ItemCountForPopup
            };
            notificationPage.AddPropertyType(itemCountForPopupProperty, "Content");

            contentService.Save(notificationPage);
            CoreInstallationStep_0_0_1.AddAllowedChildNode(CoreInstallationConstants.DocumentTypeAliases.HomePage, NotificationInstallationConstants.DocumentTypeAliases.NotificationPage);
        }
    }
}
