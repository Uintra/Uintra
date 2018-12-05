using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Indexes
{
    public class ElasticUintraContentIndex : IElasticUintraContentIndex, IElasticEntityMapper
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

        public bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }
    }
}