using System;
using System.Collections.Generic;
using Compent.Uintra.Core.UserTags.Indexers;

namespace Compent.Uintra.Core.Search.Indexes
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
    }
}