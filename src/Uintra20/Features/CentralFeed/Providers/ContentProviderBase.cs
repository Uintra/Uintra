using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed.Providers
{
    public abstract class ContentProviderBase
    {
	    //protected virtual IPublishedContent GetContent(IEnumerable<string> xPath) =>
        //    _umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(xPath));

        protected virtual IPublishedContent GetContent(IEnumerable<string> aliasesPath)
        {
            IEnumerable<IPublishedContent> targetContents = Umbraco.Web.Composing.Current.UmbracoHelper.ContentAtRoot();

            foreach (var alias in aliasesPath)
            {
                targetContents = targetContents.Where(x => x.ContentType.Alias == alias).SelectMany(x => x.Children);
            }

            return targetContents.FirstOrDefault();
        }

        protected virtual IEnumerable<IPublishedContent> GetDescendants(IEnumerable<string> aliasesPath)
        {
            return GetContent(aliasesPath).Children;
        }
    }
}