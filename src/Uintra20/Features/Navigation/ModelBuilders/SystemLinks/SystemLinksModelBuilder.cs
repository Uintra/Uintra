using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Services;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Navigation.ModelBuilders.SystemLinks
{
    public class SystemLinksModelBuilder : ISystemLinksModelBuilder
    {
        private readonly ISystemLinksService _systemLinksService;

        public SystemLinksModelBuilder(ISystemLinksService systemLinksService)
        {
            _systemLinksService = systemLinksService;
        }

        public IEnumerable<SystemLinksModel> Get(
            IEnumerable<string> contentAliasPath,
            string titleNodePropertyAlias,
            string linksNodePropertyAlias,
            string sortOrderNodePropertyAlias,
            Func<SystemLinksModel, int> sort)
        {
            var result = Enumerable.Empty<SystemLinksModel>();

            if (!contentAliasPath.Any())
            {
                return result;
            }

            result = _systemLinksService.GetMany(contentAliasPath)
                    .Select(x => ParseToSystemLinksModel(titleNodePropertyAlias, linksNodePropertyAlias, sortOrderNodePropertyAlias, x))
                    .OrderBy(sort);

            return result;
        }

        private SystemLinksModel ParseToSystemLinksModel(string titleNodePropertyAlias,
            string linksNodePropertyAlias, string sortOrderNodePropertyAlias, IPublishedContent content)
        {
            var result = new SystemLinksModel();

            var json = content.Value<string>(linksNodePropertyAlias);

            result.LinksGroupTitle = content.Value<string>(titleNodePropertyAlias);
            result.SortOrder = content.Value<int>(sortOrderNodePropertyAlias);
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
            catch (Exception ex) { }

            return result;
        }
    }
}