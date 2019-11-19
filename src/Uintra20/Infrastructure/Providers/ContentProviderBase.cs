using System.Collections.Generic;
using Uintra20.Features;
using Uintra20.Infrastructure.Helpers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Infrastructure.Providers
{
    public abstract class ContentProviderBase
    {
        private readonly UmbracoHelper _umbracoHelper;

        protected ContentProviderBase(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        protected virtual IPublishedContent GetContent(IEnumerable<string> xPath) =>
            _umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(xPath));

        protected virtual IEnumerable<IPublishedContent> GetDescendants(IEnumerable<string> xPath) =>
            _umbracoHelper.ContentAtXPath(XPathHelper.GetDescendantsXpath(xPath));
    }
}