using uIntra.Core;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core
{
    public class IntranetUserContentProvider : IIntranetUserContentProvider
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public IntranetUserContentProvider(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetProfilePage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetProfilePage()));
        }

        public IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetProfileEditPage()));
        }
    }
}