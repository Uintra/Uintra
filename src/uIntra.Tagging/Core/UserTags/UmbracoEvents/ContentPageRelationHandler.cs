using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace uIntra.Tagging.UserTags
{
    public class ContentPageRelationHandler : IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService
    {
        private readonly IUserTagService _userTagService;
        private readonly IGridHelper _gridHelper;

        public ContentPageRelationHandler(IUserTagService userTagService, IGridHelper gridHelper)
        {
            _userTagService = userTagService;
            _gridHelper = gridHelper;
        }

        public void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> args)
        {
            var contentPagesWithTags = ParseUserTags(args.PublishedEntities);
        }

        // TODO
        private IEnumerable<(IContent content, IEnumerable<Guid>)> ParseUserTags(IEnumerable<IContent> affectedContent)
        {
            foreach (var content in affectedContent)
            {
                if (content.ContentType.Alias == "contentPage")
                {
                    var json = content.GetValue<string>("grid");
                    var grid = JObject.Parse(json);
                    var systemTags = _gridHelper.GetValues(grid, "custom.UsersTags").FirstOrDefault();
                }
            }

            return null;
        }

        public void ProcessContentUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            throw new System.NotImplementedException();
        }
    }
}
