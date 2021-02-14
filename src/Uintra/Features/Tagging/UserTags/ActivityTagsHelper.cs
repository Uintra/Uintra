using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra.Core.Search.Indexes;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Tagging.UserTags
{
    public class ActivityTagsHelper : IActivityTagsHelper
    {
        private readonly IUserTagService _userTagService;
        private readonly IActivityUserTagSearchRepository _userTagSearchRepository;
        private readonly IUserTagProvider _userTagProvider;

        public ActivityTagsHelper(IUserTagService userTagService, IActivityUserTagSearchRepository userTagSearchRepository, IUserTagProvider userTagProvider)
        {
            _userTagService = userTagService;
            _userTagSearchRepository = userTagSearchRepository;
            _userTagProvider = userTagProvider;
        }

        public void ReplaceTags(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            _userTagService.Replace(entityId, tagIds);
            _userTagSearchRepository.Update(entityId, tags.Select(t => t.Text));
        }

        public void ReplaceTags(Guid entityId, IEnumerable<Guid> collection)
        {
            var tags = _userTagProvider.Get(collection);
            _userTagService.Replace(entityId, collection);
            _userTagSearchRepository.Update(entityId, tags.Select(t => t.Text));
        }

        public async Task ReplaceTagsAsync(Guid entityId, string collectionString)
        {
            var tagIds = collectionString.ParseStringCollection(Guid.Parse).ToList();
            var tags = _userTagProvider.Get(tagIds);

            await _userTagService.ReplaceAsync(entityId, tagIds);
            _userTagSearchRepository.Update(entityId, tags.Select(t => t.Text));
        }

        public async Task ReplaceTagsAsync(Guid entityId, IEnumerable<Guid> collection)
        {
            var tags = _userTagProvider.Get(collection);
            await _userTagService.ReplaceAsync(entityId, collection);
            _userTagSearchRepository.Update(entityId, tags.Select(t => t.Text));
        }
    }
}