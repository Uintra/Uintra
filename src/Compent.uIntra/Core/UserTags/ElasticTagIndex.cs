using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Search;

namespace Compent.uIntra.Core.UserTags
{
    public class ElasticTagIndex : IElasticTagIndex
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
    }
}