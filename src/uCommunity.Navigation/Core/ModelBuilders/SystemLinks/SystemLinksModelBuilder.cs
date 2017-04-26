using System;
using System.Linq;
using ServiceStack;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public class SystemLinksModelBuilder : ISystemLinksModelBuilder
    {
        private readonly ISystemLinksService _systemLinksService;

        public SystemLinksModelBuilder(ISystemLinksService systemLinksService)
        {
            _systemLinksService = systemLinksService;
        }

        public SystemLinksModel Get(string contentXPath, string titleNodePropertyAlias, string urlNodePropertyAlias, Func<SystemLinkItemModel, int> sort)
        {
            var result = new SystemLinksModel();

            if (contentXPath.IsNullOrEmpty())
            {
                return result;
            }

            result.SystemLinks =
                _systemLinksService.GetMany(contentXPath)
                    .Select(x => ParseToSystemLinkItemModel(titleNodePropertyAlias, urlNodePropertyAlias, x))
                    .OrderBy(sort)
                    .ToList();

            return result;
        }

        private SystemLinkItemModel ParseToSystemLinkItemModel(string titleNodePropertyAlias,
            string urlNodePropertyAlias, IPublishedContent content)
        {
            var result = new SystemLinkItemModel();

            result.Name = content.GetPropertyValue<string>(titleNodePropertyAlias);
            result.Url = content.GetPropertyValue<string>(urlNodePropertyAlias);
            result.SortOrder = content.SortOrder;

            return result;
        }
    }
}