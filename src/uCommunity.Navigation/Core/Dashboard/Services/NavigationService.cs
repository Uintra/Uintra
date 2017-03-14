using System.Web;
using uCommunity.Core.App_Plugins.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uCommunity.Navigation.Core.Dashboard
{
    public class NavigationService : INavigationService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        private readonly NavigationConfiguration _navigationConfiguration;

        public NavigationService(
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider)
        {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _navigationConfiguration = navigationConfigurationProvider.GetSettings();
        }

        public void CreateNavigationComposition(string folderId)
        {
            if (IsNavigationCompositionExist())
            {
                return;
            }

            var parentId = GetParentId(folderId);
            if (!parentId.HasValue)
            {
                return;
            }

            var navigationCompositionContentType = new ContentType(parentId.Value)
            {
                Alias = _navigationConfiguration.NavigationCompositionAlias,
                Name = _navigationConfiguration.NavigationCompositionAlias
            };

            CreateNavigationTab(navigationCompositionContentType);
            AddNavigationCompositionProperties(navigationCompositionContentType);

            _contentTypeService.Save(navigationCompositionContentType);
        }

        public void CreateHomeNavigationComposition(string folderId)
        {
            if (IsHomeNavigationCompositionExist())
            {
                return;
            }

            var parentId = GetParentId(folderId);
            if (!parentId.HasValue)
            {
                return;
            }

            var navigationHomeCompositionContentType = new ContentType(parentId.Value)
            {
                Alias = _navigationConfiguration.HomeNavigationCompositionAlias,
                Name = _navigationConfiguration.HomeNavigationCompositionAlias
            };

            CreateNavigationTab(navigationHomeCompositionContentType);
            AddNavigationCompositionProperties(navigationHomeCompositionContentType);

            _contentTypeService.Save(navigationHomeCompositionContentType);
        }

        public bool IsNavigationCompositionExist()
        {
            var navigationCompositionContentType = _contentTypeService.GetContentType(_navigationConfiguration.NavigationCompositionAlias);
            return navigationCompositionContentType != null;
        }

        public bool IsHomeNavigationCompositionExist()
        {
            var homeNavigationCompositionContentType = _contentTypeService.GetContentType(_navigationConfiguration.HomeNavigationCompositionAlias);
            return homeNavigationCompositionContentType != null;
        }

        private int? GetParentId(string folderId)
        {
            var mailTemplateParentIdOrAlias = HttpUtility.HtmlEncode(folderId);

            if (string.IsNullOrWhiteSpace(mailTemplateParentIdOrAlias))
            {
                return -1;
            }

            int parentId;
            if (int.TryParse(mailTemplateParentIdOrAlias, out parentId))
            {
                return parentId;
            }

             //_contentTypeService.GetContentType(parentId);

            return parentId;
        }

        private void CreateNavigationTab(IContentType contentType)
        {
            contentType.AddPropertyGroup(_navigationConfiguration.NavigationTab);
        }

        private void AddNavigationCompositionProperties(IContentType navigationCompositionContentType)
        {
            AddProperty(navigationCompositionContentType, UmbracoPropertyTypeEnum.Textstring, _navigationConfiguration.NavigationName.Alias,
                _navigationConfiguration.NavigationName.Name, sortOrder: 0, mandatory: true, tab: _navigationConfiguration.NavigationTab);

            AddProperty(navigationCompositionContentType, UmbracoPropertyTypeEnum.TrueFalse, _navigationConfiguration.IsHideFromNavigation.Alias,
                _navigationConfiguration.IsHideFromNavigation.Name, sortOrder: 1, mandatory: true, tab: _navigationConfiguration.NavigationTab);

            AddProperty(navigationCompositionContentType, UmbracoPropertyTypeEnum.TrueFalse, _navigationConfiguration.IsShowInLeftNavigation.Alias,
               _navigationConfiguration.IsShowInLeftNavigation.Name, sortOrder: 2, mandatory: true, tab: _navigationConfiguration.NavigationTab);

            AddProperty(navigationCompositionContentType, UmbracoPropertyTypeEnum.TrueFalse, _navigationConfiguration.IsShowInSubNavigation.Alias,
               _navigationConfiguration.IsShowInSubNavigation.Name, sortOrder: 3, mandatory: true, tab: _navigationConfiguration.NavigationTab);
        }

        private void AddProperty(IContentType mailTemplateDocType, UmbracoPropertyTypeEnum propertyTypeEnum, string propertyAlias, string propertyName, int sortOrder, bool mandatory, string tab, string description = "")
        {
            var dateDataType = GetDataTypeDefinitionByEnum(propertyTypeEnum);
            var property = new PropertyType(dateDataType)
            {
                Alias = propertyAlias,
                Name = propertyName,
                Mandatory = mandatory,
                SortOrder = sortOrder,
                Description = description
            };

            mailTemplateDocType.AddPropertyType(property, tab);
        }

        private IDataTypeDefinition GetDataTypeDefinitionByEnum(UmbracoPropertyTypeEnum propertyTypeEnum)
        {
            var result = _dataTypeService.GetDataTypeDefinitionById((int)propertyTypeEnum);
            return result;
        }
    }
}
