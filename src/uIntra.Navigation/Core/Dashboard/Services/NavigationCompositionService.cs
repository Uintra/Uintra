using System.Linq;
using uIntra.Core.Configuration;
using uIntra.Navigation.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Navigation.Dashboard
{
    public class NavigationCompositionService : DocumentTypeService, INavigationCompositionService
    {
        private readonly NavigationConfiguration _navigationConfiguration;

        public NavigationCompositionService(
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider
            ) : base(contentTypeService, dataTypeService)
        {
            _navigationConfiguration = navigationConfigurationProvider.GetSettings();
        }

        protected override void CreateTabs(IContentType currentDocumentType, IContentType parentDocumentType)
        {
            if (parentDocumentType == null || !parentDocumentType.CompositionPropertyGroups.Any(x => x.Name.Equals(_navigationConfiguration.NavigationTab)))
            {
                currentDocumentType.AddPropertyGroup(_navigationConfiguration.NavigationTab);
            }
        }

        protected override void AddProperties(IContentType documentType)
        {
            AddProperty(documentType, UmbracoPropertyTypeEnum.Textstring, _navigationConfiguration.NavigationName.Alias,
                 _navigationConfiguration.NavigationName.Name, sortOrder: 0, mandatory: true, tab: _navigationConfiguration.NavigationTab);

            AddProperty(documentType, UmbracoPropertyTypeEnum.TrueFalse, _navigationConfiguration.IsHideFromLeftNavigation.Alias,
                _navigationConfiguration.IsHideFromLeftNavigation.Name, sortOrder: 1, mandatory: false, tab: _navigationConfiguration.NavigationTab);

            AddProperty(documentType, UmbracoPropertyTypeEnum.TrueFalse, _navigationConfiguration.IsHideFromSubNavigation.Alias,
               _navigationConfiguration.IsHideFromSubNavigation.Name, sortOrder: 4, mandatory: false, tab: _navigationConfiguration.NavigationTab);
        }

        protected override string Alias => _navigationConfiguration.NavigationComposition.Alias;
        protected override string Name => _navigationConfiguration.NavigationComposition.Name;
    }
}
