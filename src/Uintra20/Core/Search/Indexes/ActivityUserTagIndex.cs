using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;

namespace Uintra20.Core.Search.Indexes
{
    public class ActivityUserTagIndex : IActivityUserTagIndex
    {
        private readonly IElasticUintraActivityIndex _activityIndex;

        public ActivityUserTagIndex(IElasticUintraActivityIndex activityIndex)
        {
            _activityIndex = activityIndex;
        }

        public void Update(Guid activityId, IEnumerable<string> tagNames)
        {
            var activity = _activityIndex.Get(activityId);
            if (activity == null) return;

            activity.UserTagNames = tagNames;
            _activityIndex.Index(activity);
        }

        public void Remove(Guid activityId, IEnumerable<string> tagNames)
        {
            var activity = _activityIndex.Get(activityId);
            if (activity == null) return;
            activity.UserTagNames = activity.UserTagNames.Except(tagNames);
            _activityIndex.Index(activity);
        }

        public void Add(Guid activityId, string tagName)
        {
            var activity = _activityIndex.Get(activityId);
            if (activity == null) return;
            activity.UserTagNames = activity.UserTagNames.Union(tagName.ToEnumerable());
            _activityIndex.Index(activity);
        }
    }
}