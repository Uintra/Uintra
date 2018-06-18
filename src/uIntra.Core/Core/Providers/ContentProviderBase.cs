using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core
{
    public abstract class ContentProviderBase 
    {
        private readonly UmbracoHelper _umbracoHelper;

        protected ContentProviderBase(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        protected virtual IPublishedContent GetContent(IEnumerable<string> xPath) =>
            _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(xPath));
    }
}