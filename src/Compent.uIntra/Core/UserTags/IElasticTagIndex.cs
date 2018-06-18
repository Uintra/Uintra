using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;

namespace Compent.Uintra.Core.UserTags
{
    public interface IElasticTagIndex
    {
        void Index(SearchableTag tag);
        void Index(IEnumerable<SearchableTag> tags);
        void Delete(Guid id);
        void Delete();
    }
}