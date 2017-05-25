using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Navigation.Dashboard
{
    public abstract class DocumentTypeService : IDocumentTypeService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        protected DocumentTypeService(
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService
            )
        {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
        }

        protected abstract string Alias { get; }
        protected abstract string Name { get; }
        protected abstract void CreateTabs(IContentType currentDocumentType, IContentType parentDocumentType);
        protected abstract void AddProperties(IContentType documentType);

        protected void AddProperty(IContentType mailTemplateDocType, UmbracoPropertyTypeEnum propertyTypeEnum, string propertyAlias, string propertyName, int sortOrder, bool mandatory, string tab, string description = "")
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

        public CreateDocumentTypeState Create(int? parentId)
        {
            var result = new CreateDocumentTypeState
            {
                IsExists = false
            };

            if (IsExists())
            {
                result.IsExists = true;
                return result;
            }

            if (!parentId.HasValue)
            {
                result.IsUnknownParent = true;
                return result;
            }

            var documentType = new ContentType(parentId.Value)
            {
                Alias = Alias,
                Name = Name
            };

            CreateTabsAddTabsFromParent(documentType);
            AddProperties(documentType);

            _contentTypeService.Save(documentType);

            result.IsExists = true;
            return result;
        }

        public CreateDocumentTypeState Create(string parentIdOrAlias)
        {
            var parentId = GetParentId(parentIdOrAlias);
            var result = Create(parentId);

            return result;
        }

        public DocumentTypeState Delete()
        {
            var result = new DocumentTypeState();

            var homeNavigationComposition = _contentTypeService.GetContentType(Alias);
            if (homeNavigationComposition == null)
            {
                result.IsExists = false;
                return result;
            }

            _contentTypeService.Delete(homeNavigationComposition);

            result.IsExists = false;
            return result;
        }

        public bool IsExists()
        {
            var documentType = _contentTypeService.GetContentType(Alias);
            return documentType != null;
        }

        private int? GetParentId(string parentIdOrAlias)
        {
            var parentIdOrAliasEncoded = HttpUtility.HtmlEncode(parentIdOrAlias);

            if (string.IsNullOrWhiteSpace(parentIdOrAliasEncoded))
            {
                return -1;
            }

            int parentId;
            if (int.TryParse(parentIdOrAliasEncoded, out parentId))
            {
                return parentId;
            }

            return _contentTypeService.GetContentType(parentIdOrAliasEncoded)?.Id;
        }

        private void CreateTabsAddTabsFromParent(IContentType documenType)
        {
            var parentDocumentType = _contentTypeService.GetContentType(documenType.ParentId);
            if (parentDocumentType != null)
            {
                documenType.AddContentType(parentDocumentType);
            }

            CreateTabs(documenType, parentDocumentType);
        }

        private IDataTypeDefinition GetDataTypeDefinitionByEnum(UmbracoPropertyTypeEnum propertyTypeEnum)
        {
            var result = _dataTypeService.GetDataTypeDefinitionById((int)propertyTypeEnum);
            return result;
        }
    }
}
