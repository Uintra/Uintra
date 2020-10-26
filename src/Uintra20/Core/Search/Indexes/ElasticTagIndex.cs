using System;
using System.Collections.Generic;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Repository;

namespace Uintra20.Core.Search.Indexes
{
    public class ElasticTagIndex : IElasticTagIndex, IElasticEntityMapper
    {
        private readonly IElasticSearchRepository<SearchableTag> _elasticSearchRepository;

        public ElasticTagIndex(IElasticSearchRepository<SearchableTag> elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public void Index(SearchableTag tag)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(tag);
        }

        public void Index(IEnumerable<SearchableTag> tags)
        {
            _elasticSearchRepository.EnsureMappingExist();
            _elasticSearchRepository.Save(tags);
        }

        public void Delete(Guid id)
        {
            _elasticSearchRepository.Delete(id);
        }

        public void Delete()
        {
            _elasticSearchRepository.DeleteAllByType(UintraSearchableTypeEnum.Tag);
        }

        public bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }
    }
}