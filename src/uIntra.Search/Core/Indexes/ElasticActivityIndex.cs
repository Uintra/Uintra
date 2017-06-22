using System;
using System.Collections.Generic;

namespace uIntra.Search.Core
{
    public class ElasticActivityIndex : IElasticActivityIndex
    {
        private readonly IElasticSearchRepository<SearchableActivity> _elasticSearchRepository;

        public ElasticActivityIndex(
            IElasticSearchRepository<SearchableActivity> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public void Index(SearchableActivity activity)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activity);
        }

        public void Index(IEnumerable<SearchableActivity> activities)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activities);
        }

        public void Delete(Guid id)
        {
            _elasticSearchRepository.Delete(id);
        }

        public void DeleteByType(SearchableType type)
        {
            _elasticSearchRepository.DeleteAllByType(type);
        }
    }
}