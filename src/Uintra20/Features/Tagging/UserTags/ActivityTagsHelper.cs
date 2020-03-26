using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Tagging.UserTags
{
    public class ActivityTagsHelper : IActivityTagsHelper
    {
        private readonly IUserTagService _userTagService;
        private readonly IActivityUserTagIndex _userTagIndex;
        private readonly IUserTagProvider _userTagProvider;

        public ActivityTagsHelper(IUserTagService userTagService, IActivityUserTagIndex userTagIndex, IUserTagProvider userTagProvider)
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

        public void ReplaceTags(Guid entityId, IEnumerable<Guid> collection)
        {
            var tags = _userTagProvider.Get(collection);
            _userTagService.Replace(entityId, collection);
            _userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }

        public async Task ReplaceTagsAsync(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            await _userTagService.ReplaceAsync(entityId, tagIds);
            _userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }

        public async Task ReplaceTagsAsync(Guid entityId, IEnumerable<Guid> collection)
        {
            var tags = _userTagProvider.Get(collection);
            await _userTagService.ReplaceAsync(entityId, collection);
            _userTagIndex.Update(entityId, tags.Select(t => t.Text));
        }
    }
}