using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;

namespace Compent.uIntra.Core.UserTags
{
    public interface IElasticTagIndex
    {
        void Index(SearchableTag tag);
        void Index(IEnumerable<SearchableTag> tags);
        void Delete(Guid id);
        void Delete();
    }
}