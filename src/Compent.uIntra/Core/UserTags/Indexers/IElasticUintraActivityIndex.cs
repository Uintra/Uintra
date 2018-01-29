using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Core.TypeProviders;

namespace Compent.uIntra.Core.UserTags.Indexers
{
    public interface IElasticUintraActivityIndex
    {
        SearchableUintraActivity Get(Guid id);
        void Index(SearchableUintraActivity activity);
        void Index(IEnumerable<SearchableUintraActivity> activities);
        void Delete(Guid id);
        void DeleteByType(IIntranetType type);
    }
}