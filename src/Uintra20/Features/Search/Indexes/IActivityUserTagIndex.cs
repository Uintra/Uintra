using System;
using System.Collections.Generic;

namespace Uintra20.Features.Search.Indexes
{
    public interface IActivityUserTagIndex
    {
        void Update(Guid activityId, IEnumerable<string> tagNames);
    }
}