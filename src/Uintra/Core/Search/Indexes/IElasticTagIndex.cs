using System;
using System.Collections.Generic;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Indexes
{
    public interface IElasticTagIndex
    {
        void Index(SearchableTag tag);
        void Index(IEnumerable<SearchableTag> tags);
        void Delete(Guid id);
        void Delete();
    }
}