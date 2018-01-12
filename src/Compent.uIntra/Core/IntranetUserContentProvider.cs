using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Extensions;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core
{
    public class IntranetUserContentProvider : ContentProviderBase, IIntranetUserContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IEnumerable<string> _baseXPath;

        public IntranetUserContentProvider(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _baseXPath = new[] { _documentTypeAliasProvider.GetHomePage() };
        }

        public IPublishedContent GetProfilePage()
        {
            var xPath = _baseXPath.Append(_documentTypeAliasProvider.GetProfilePage());
            return GetContent(xPath);
        }

        public IPublishedContent GetEditPage()
        {
            var xPath = _baseXPath.Append(_documentTypeAliasProvider.GetProfileEditPage());
            return GetContent(xPath);
        }
    }
}