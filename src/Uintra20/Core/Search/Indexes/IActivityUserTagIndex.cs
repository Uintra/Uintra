﻿using System;
using System.Collections.Generic;

namespace Uintra20.Core.Search.Indexes
{
    public interface IActivityUserTagIndex
    {
        void Update(Guid activityId, IEnumerable<string> tagNames);
        void Remove(Guid activityId, IEnumerable<string> tagNames);
        void Add(Guid activityId, string tagName);
    }
}