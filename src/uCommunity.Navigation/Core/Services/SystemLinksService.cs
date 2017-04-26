using System.Collections.Generic;
using uCommunity.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public class SystemLinksService : ISystemLinksService
    {
        private readonly UmbracoHelper _umbracoHelper;

        public SystemLinksService(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IEnumerable<IPublishedContent> GetMany(string xPath)
        {
            return _umbracoHelper.TypedContentAtXPath(XPathHelper.GetXpath(xPath));
        }
    }
}