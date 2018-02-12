using System;
using System.Collections.Generic;

namespace Compent.Uintra.Core.Search.Indexes
{
    public interface IActivityUserTagIndex
    {
        void Update(Guid activityId, IEnumerable<string> tagNames);
    }
}