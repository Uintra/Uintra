using System.Collections.Generic;
using System.Linq;
using Uintra20.Infrastructure.Helpers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Providers
{
    public abstract class ContentProviderBase
    {
        private readonly UmbracoHelper _umbracoHelper;

        protected ContentProviderBase(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        //protected virtual IPublishedContent GetContent(IEnumerable<string> xPath) =>
        //    _umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(xPath));

        protected virtual IPublishedContent GetContent(IEnumerable<string> aliasesPath)
        {
            IEnumerable<IPublishedContent> targetContents = _umbracoHelper.ContentAtRoot();

            foreach (var alias in aliasesPath)
            {
                targetContents = targetContents.Where(x => x.ContentType.Alias == alias).SelectMany(x => x.Children);
            }

            return targetContents.FirstOrDefault();
        }

        //protected virtual IEnumerable<IPublishedContent> GetDescendants(IEnumerable<string> xPath) =>
        //    _umbracoHelper.ContentAtXPath(XPathHelper.GetDescendantsXpath(xPath));
    }
}