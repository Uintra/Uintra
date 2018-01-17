using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using uIntra.Core.Grid;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace uIntra.Tagging.UserTags
{
    public class ContentPageRelationHandler : IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService, IUmbracoContentTrashedEventService
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

            foreach (var (_, tagIds, entityId) in contentPagesWithTags)
            {
                _userTagService.Replace(entityId, tagIds);
            }
        }

        private IEnumerable<(IContent content, IEnumerable<Guid> tagIds, Guid entityId)> ParseUserTags(IEnumerable<IContent> affectedContent)
        {
            foreach (var content in affectedContent)
            {
                var tags = new List<Guid>();
                if (content.ContentType.Alias == "contentPage")
                {
                    var json = content.GetValue<string>("grid");
                    var grid = JObject.Parse(json);
                    var data = _gridHelper.GetValues(grid, "custom.UsersTags").FirstOrDefault();


                    foreach (var systemTag in data.value.usersTags)
                    {
                        if ((bool) systemTag.selected)
                        {
                            tags.Add(Guid.Parse((string) systemTag.id));
                        }
                    }

                    yield return (content, tags, content.Key);
                }
            }
        }

        public void ProcessContentUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {

        }

        public void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args)
        {
            var content = args.MoveInfoCollection.Select(info => info.Entity);
            var contentPagesWithTags = ParseUserTags(content);

            foreach (var (_, _, entityId) in contentPagesWithTags)
            {
                _userTagService.DeleteAllFor(entityId);
            }
        }
    }
}
