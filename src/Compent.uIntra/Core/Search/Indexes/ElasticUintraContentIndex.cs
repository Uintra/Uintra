using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Indexes
{
    public class ElasticUintraContentIndex : IElasticUintraContentIndex
    {
        private readonly IElasticSearchRepository<SearchableUintraContent> _elasticSearchRepository;

        public ElasticUintraContentIndex(
            IElasticSearchRepository<SearchableUintraContent> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public SearchableUintraContent Get(int id)
        {
            return _elasticSearchRepository.Get(id);
        }

        public void Index(SearchableUintraContent activity)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activity);
        }

        public void Index(IEnumerable<SearchableUintraContent> activities)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activities);
        }

        public void Delete(int id)
        {
            _elasticSearchRepository.Delete(id);
        }
    }
}