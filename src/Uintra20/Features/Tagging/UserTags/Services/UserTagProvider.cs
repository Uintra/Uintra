using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public class UserTagProvider : IUserTagProvider
    {
	    private readonly INodeModelService _nodeModelService;

        public UserTagProvider(INodeModelService nodeModelService)
        {
	        _nodeModelService = nodeModelService;
        }

        public virtual UserTag Get(Guid tagId)
        {
            var umbracoNode = Umbraco.Web.Composing.Current.UmbracoHelper.Content(tagId);
            return Map(umbracoNode);
        }

        public virtual IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds)
        {
            return GetAll().Join(tagIds, t => t.Id, id => id, (t, _) => t);
        }

        public virtual IEnumerable<UserTag> GetAll()
        {
            return _nodeModelService
                .AsEnumerable()
                .OfType<UserTagItemModel>()
                .Select(t =>
                {
                    var text = _nodeModelService.GetViewModel<UserTagItemViewModel>(t)?.Text?.Value;
                    return new UserTag(t.Key, text);
                });
        }

        protected virtual UserTag Map(IPublishedContent userTag)
        {
            var id = userTag.Key;
            var text = userTag.Value<string>(UmbracoAliases.Tags.TagText);
            return new UserTag(id, text);
        }
    }
}