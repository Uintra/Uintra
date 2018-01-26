using System;
using System.Collections.Generic;

namespace Compent.uIntra.Core.Search.Indexes
{
    public interface IActivityUserTagIndex
    {
        void Update(Guid activityId, IEnumerable<string> tagNames);
    }
}