using System;
using System.Collections.Generic;
using Nest;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Repository
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
        bool CreateMap(out string error);
        void DeleteAllByType(Enum type);
    }
}