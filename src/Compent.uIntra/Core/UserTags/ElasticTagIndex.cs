using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Core.Extensions;
using uIntra.Search;

namespace Compent.uIntra.Core.UserTags
{
    public class ElasticTagIndex : IElasticTagIndex
    {
        private readonly IElasticSearchRepository<SearchableTag> _elasticSearchRepository;
        private readonly ISearchableTypeProvider _searchableTypeProvider;

        public ElasticTagIndex(
            IElasticSearchRepository<SearchableTag> elasticSearchRepository,
            ISearchableTypeProvider searchableTypeProvider)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _searchableTypeProvider = searchableTypeProvider;
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
            _elasticSearchRepository.DeleteAllByType(_searchableTypeProvider.Get(UintraSearchableTypeEnum.Tag.ToInt()));
        }
    }
}