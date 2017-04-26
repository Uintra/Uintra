using System;
using System.Linq;
using ServiceStack;

namespace uCommunity.Navigation.Core
{
    public class SystemLinksModelBuilder : ISystemLinksModelBuilder
    {
        private readonly ISystemLinksService _systemLinksService;

        public SystemLinksModelBuilder(ISystemLinksService systemLinksService)
        {
            _systemLinksService = systemLinksService;
        }

        public SystemLinksModel Get(string systemLinksContentXPath, string pageTitleNodePropertyAlias, string pageUrlNodePropertyAlias, Func<SystemLinkItemModel, string> sort)
        {
            var result = new SystemLinksModel();

            if (systemLinksContentXPath.IsNullOrEmpty())
            {
                return result;
            }

            var systemLinks = _systemLinksService.GetMany(systemLinksContentXPath);

            foreach (var link in systemLinks)
            {
                result.SystemLinks.Add(new SystemLinkItemModel(pageTitleNodePropertyAlias, pageUrlNodePropertyAlias, link));
            }

            result.SystemLinks = result.SystemLinks.OrderBy(sort).ToList();

            return result;
        }
    }
}
