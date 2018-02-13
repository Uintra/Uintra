using System;
using Uintra.Core;
using Umbraco.Web;

namespace Compent.Uintra.Core.Helpers
{
    public class UmbracoContentHelper : IUmbracoContentHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public UmbracoContentHelper(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public bool IsForContentPage(Guid id)
        {
            return _umbracoHelper.TypedContent(id)?.DocumentTypeAlias == _documentTypeAliasProvider.GetContentPage();
        }
    }
}