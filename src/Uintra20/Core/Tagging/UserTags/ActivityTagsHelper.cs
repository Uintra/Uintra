using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Core.Tagging.UserTags;

namespace Uintra20.Core.UserTags
{
    public class ActivityTagsHelper : IActivityTagsHelper
    {
        private readonly UserTagService _userTagService;
        //private readonly IActivityUserTagIndex _userTagIndex;
        private readonly IUserTagProvider _userTagProvider;

        public ActivityTagsHelper(UserTagService userTagService, /*IActivityUserTagIndex userTagIndex,*/ IUserTagProvider userTagProvider)
        {
            _userTagService = userTagService;
            //_userTagIndex = userTagIndex;
            _userTagProvider = userTagProvider;
        }

        public void ReplaceTags(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            _userTagService.Replace(entityId, tagIds);
            //_userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }

        public async Task ReplaceTagsAsync(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            await _userTagService.ReplaceAsync(entityId, tagIds);
            //_userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }
    }
}