using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Navigation.SystemLinks
{
    public class SystemLinksModelBuilder : ISystemLinksModelBuilder
    {
        private readonly ISystemLinksService _systemLinksService;

        public SystemLinksModelBuilder(ISystemLinksService systemLinksService)
        {
            _systemLinksService = systemLinksService;
        }

        public IEnumerable<SystemLinksModel> Get(
            string contentXPath,
            string titleNodePropertyAlias,
            string linksNodePropertyAlias,
            string sortOrderNodePropertyAlias,
            Func<SystemLinksModel, int> sort)
        {
            var result = Enumerable.Empty<SystemLinksModel>();

            if (contentXPath.IsNullOrEmpty())
            {
                return result;
            }

            result = _systemLinksService.GetMany(contentXPath)
                    .Select(x => ParseToSystemLinksModel(titleNodePropertyAlias, linksNodePropertyAlias, sortOrderNodePropertyAlias, x))
                    .OrderBy(sort);

            return result;
        }

        private SystemLinksModel ParseToSystemLinksModel(string titleNodePropertyAlias,
            string linksNodePropertyAlias, string sortOrderNodePropertyAlias, IPublishedContent content)
        {
            var result = new SystemLinksModel();

            var json = content.GetPropertyValue<string>(linksNodePropertyAlias);

            result.LinksGroupTitle = content.GetPropertyValue<string>(titleNodePropertyAlias);
            result.SortOrder = content.GetPropertyValue<int>(sortOrderNodePropertyAlias);
            result.SystemLinks = ParseToSystemLinkItems(json);

            return result;
        }

        private List<SystemLinkItemModel> ParseToSystemLinkItems(string json)
        {
            
            var result = new List<SystemLinkItemModel>();

            try
            {
                result = JsonConvert.DeserializeObject<List<SystemLinkItemModel>>(json);
            }
            catch(Exception ex) { }

            return result;
        }
   }
}