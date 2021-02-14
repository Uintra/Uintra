using Compent.Shared.Search.Elasticsearch;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteSearchableActivityByTypeSpecificationFactory : IDeleteSpecificationFactory<SearchableActivity, DeleteSearchableActivityByTypeQuery>
    {
        public DeleteQuerySpecification<SearchableActivity> Create(DeleteSearchableActivityByTypeQuery query, string culture)
        {
            var spec = new DeleteSearchableActivityByTypeSpecification(query);
            return spec;
        }
    }
}