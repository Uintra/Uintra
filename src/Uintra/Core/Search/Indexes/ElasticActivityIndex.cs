﻿using System;
using System.Collections.Generic;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Repository;

namespace Uintra.Core.Search.Indexes
{
    public class ElasticActivityIndex : IElasticActivityIndex, IElasticEntityMapper
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