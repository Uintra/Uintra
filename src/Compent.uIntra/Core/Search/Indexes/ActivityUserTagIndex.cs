using System;
using System.Collections.Generic;
using Compent.uIntra.Core.UserTags.Indexers;

namespace Compent.uIntra.Core.Search.Indexes
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
            activity.UserTagNames = tagNames;
            //_activityIndex.Delete(activityId);
            _activityIndex.Index(activity);
        }
    }
}