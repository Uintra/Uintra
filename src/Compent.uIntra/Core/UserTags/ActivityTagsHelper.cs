using System;
using System.Linq;
using Compent.Uintra.Core.Search.Indexes;
using Uintra.Core.Extensions;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.UserTags
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

            _userTagService.Replace(entityId, tagIds);
            _userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }
    }
}