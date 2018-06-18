using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Navigation
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
            return _umbracoHelper.TypedContentAtXPath(xPath);
        }
    }
}