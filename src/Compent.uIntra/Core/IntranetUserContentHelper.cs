using uIntra.Core;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core
{
    public class IntranetUserContentHelper : IIntranetUserContentHelper
    {
        private readonly UmbracoHelper _umbracoHelper;

        public IntranetUserContentHelper(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IPublishedContent GetProfilePage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, ProfilePage.ModelTypeAlias));
        }

        public IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, ProfileEditPage.ModelTypeAlias));
        }
    }
}