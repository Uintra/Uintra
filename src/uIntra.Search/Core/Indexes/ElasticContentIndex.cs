using System.Collections.Generic;

namespace Uintra.Search
{
    public class ElasticContentIndex : IElasticContentIndex, IElasticEntityMapper
    {
        private readonly IElasticSearchRepository<SearchableContent> _elasticSearchRepository;

        public ElasticContentIndex(
            IElasticSearchRepository<SearchableContent> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public void Index(SearchableContent content)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(content);
        }

        public void Index(IEnumerable<SearchableContent> content)
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