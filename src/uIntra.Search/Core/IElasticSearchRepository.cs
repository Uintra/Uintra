using System;
using System.Collections.Generic;
using Nest;
using uIntra.Core.TypeProviders;

namespace uIntra.Search
{
    public interface IElasticSearchRepository
    {
        ISearchResponse<T> SearchByIndex<T>(SearchDescriptor<T> descriptor)
            where T : class;

        void EnsureIndexExists(Func<AnalysisDescriptor, AnalysisDescriptor> analysis);

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
        void DeleteAllByType(IIntranetType type);
    }
}