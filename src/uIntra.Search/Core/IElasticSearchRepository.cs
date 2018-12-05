using System;
using System.Collections.Generic;
using Nest;
using Uintra.Core.TypeProviders;

namespace Uintra.Search
{
    public interface IElasticSearchRepository
    {
        ISearchResponse<T> SearchByIndex<T>(SearchDescriptor<T> descriptor)
            where T : class;

        bool EnsureIndexExists(Func<AnalysisDescriptor, AnalysisDescriptor> analysis, out string error);
        void DeleteIndex();
    }

    public interface IElasticSearchRepository<T> : IElasticSearchRepository
        where T : SearchableBase
    {
        T Get(object id);
        ISearchResponse<T> Search(SearchDescriptor<T> descriptor);
        void Save(T document);
        void Save(IEnumerable<T> documents);
        void Delete(object id);
        void EnsureMappingExist();
        void DeleteAllByType(Enum type);
    }
}