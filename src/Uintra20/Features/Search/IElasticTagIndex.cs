using System;
using System.Collections.Generic;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search
{
    public interface IElasticTagIndex
    {
        void Index(SearchableTag tag);
        void Index(IEnumerable<SearchableTag> tags);
        void Delete(Guid id);
        void Delete();
    }
}