using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Repository;

namespace Uintra20.Core.Search.Indexes
{
    public class ElasticUintraActivityIndex: IElasticUintraActivityIndex, IElasticEntityMapper
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

        public bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }
    }
}