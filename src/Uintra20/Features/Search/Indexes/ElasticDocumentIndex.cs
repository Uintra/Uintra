using System.Collections.Generic;
using Uintra20.Features.Search.Entities;

namespace Uintra20.Features.Search.Indexes
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