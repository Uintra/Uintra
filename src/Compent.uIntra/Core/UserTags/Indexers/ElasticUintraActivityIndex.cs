using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Search;

namespace Compent.Uintra.Core.UserTags.Indexers
{
    public class ElasticUintraActivityIndex: IElasticUintraActivityIndex
    {
        private readonly IElasticSearchRepository<SearchableUintraActivity> _elasticSearchRepository;

        public ElasticUintraActivityIndex(
            IElasticSearchRepository<SearchableUintraActivity> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public SearchableUintraActivity Get(Guid id)
        {
            return _elasticSearchRepository.Get(id);
        }

        public void Index(SearchableUintraActivity activity)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activity);
        }

        public void Index(IEnumerable<SearchableUintraActivity> activities)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(activities);
        }

        public void Delete(Guid id)
        {
            _elasticSearchRepository.Delete(id);
        }

        public void DeleteByType(Enum type)
        {
            _elasticSearchRepository.DeleteAllByType(type);
        }
    }
}