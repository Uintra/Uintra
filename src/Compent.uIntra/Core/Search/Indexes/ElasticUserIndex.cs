using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Indexes
{
    public class ElasticUserIndex : IElasticUserIndex, IElasticEntityMapper
    {
        private readonly IElasticSearchRepository<SearchableUser> _elasticSearchRepository;
        private bool _isMappingChecked;


        public ElasticUserIndex(IElasticSearchRepository<SearchableUser> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public void Index(SearchableUser user)
        {
            EnsureMappingExist();
            _elasticSearchRepository.Save(user);
        }

        public void Index(IEnumerable<SearchableUser> users)
        {
            EnsureMappingExist();
            _elasticSearchRepository.Save(users);
        }

        public void Delete(Guid id)
        {
            _elasticSearchRepository.Delete(id);
        }

        private void EnsureMappingExist()
        {
            if (_isMappingChecked)
            {
                return;
            }

            _elasticSearchRepository.EnsureMappingExist();
            _isMappingChecked = true;
        }

        public bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }
    }
}