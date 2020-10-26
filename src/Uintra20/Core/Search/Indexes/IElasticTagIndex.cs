using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticTagIndex
    {
        void Index(SearchableTag tag);
        void Index(IEnumerable<SearchableTag> tags);
        void Delete(Guid id);
        void Delete();
    }
}