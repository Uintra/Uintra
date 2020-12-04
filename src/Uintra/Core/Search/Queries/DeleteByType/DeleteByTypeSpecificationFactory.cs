using Compent.Shared.Search.Elasticsearch;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteByTypeSpecificationFactory : IDeleteSpecificationFactory<SearchableBase, DeleteByTypeQuery>
    {
        public DeleteQuerySpecification<SearchableBase> Create(DeleteByTypeQuery query, string culture)
        {
            var spec = new DeleteByTypeSpecification(query);
            return spec;
        }
    }
}