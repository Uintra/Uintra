using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Repository;

namespace Uintra.Core.Search.Indexes
{
    public class ActivityUserTagSearchRepository : IActivityUserTagSearchRepository
    {
        private readonly IUintraSearchRepository<SearchableUintraActivity> _uintraSearchRepository;
        
        public ActivityUserTagSearchRepository(IUintraSearchRepository<SearchableUintraActivity> uintraSearchRepository)
        {
            _uintraSearchRepository = uintraSearchRepository;
        }

        public void Update(Guid activityId, IEnumerable<string> tagNames)
        {
            var activity = AsyncHelpers.RunSync(() => _uintraSearchRepository.GetAsync(activityId.ToString()));
            if (activity == null) return;

            activity.UserTagNames = tagNames;
            AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(activity));
        }

        public void Remove(Guid activityId, IEnumerable<string> tagNames)
        {
            var activity = AsyncHelpers.RunSync(() => _uintraSearchRepository.GetAsync(activityId.ToString()));
            if (activity == null) return;

            activity.UserTagNames = activity.UserTagNames.Except(tagNames);
            AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(activity));
        }

        public void Add(Guid activityId, string tagName)
        {
            var activity = AsyncHelpers.RunSync(() => _uintraSearchRepository.GetAsync(activityId.ToString()));
            if (activity == null) return;

            activity.UserTagNames = activity.UserTagNames.Union(tagName.ToEnumerable());
            AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(activity));
        }
    }
}