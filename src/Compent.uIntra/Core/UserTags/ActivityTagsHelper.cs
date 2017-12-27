using System;
using System.Linq;
using Compent.uIntra.Core.Search.Indexes;
using uIntra.Core.Extensions;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Core.UserTags
{
    public class ActivityTagsHelper : IActivityTagsHelper
    {
        private readonly UserTagService _userTagService;
        private readonly IActivityUserTagIndex _userTagIndex;
        private readonly IUserTagProvider _userTagProvider;

        public ActivityTagsHelper(UserTagService userTagService, IActivityUserTagIndex userTagIndex, IUserTagProvider userTagProvider)
        {
            _userTagService = userTagService;
            _userTagIndex = userTagIndex;
            _userTagProvider = userTagProvider;
        }

        public void ReplaceTags(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            _userTagService.ReplaceRelations(entityId, tagIds);
            _userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }
    }
}