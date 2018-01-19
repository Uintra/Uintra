using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Core.TypeProviders;
using uIntra.Search;

namespace Compent.uIntra.Core.UserTags.Indexers
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

        public void DeleteByType(IIntranetType type)
        {
            _elasticSearchRepository.DeleteAllByType(type);
        }
    }
}