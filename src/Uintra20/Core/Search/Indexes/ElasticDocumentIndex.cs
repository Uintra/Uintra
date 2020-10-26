using System.Collections.Generic;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Repository;

namespace Uintra20.Core.Search.Indexes
{
    public class ElasticDocumentIndex : IElasticDocumentIndex, IElasticEntityMapper
    {
        private readonly IElasticSearchRepository<SearchableDocument> _elasticSearchRepository;

        public ElasticDocumentIndex(IElasticSearchRepository<SearchableDocument> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public void Index(SearchableDocument content)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(content);
        }

        public void Index(IEnumerable<SearchableDocument> content)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(content);
        }

        public void Delete(int id)
        {
            _elasticSearchRepository.Delete(id);
        }

        public bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }
    }
}