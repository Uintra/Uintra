using Compent.Shared.Search.Elasticsearch;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteByTypeSpecification : DeleteQuerySpecification<SearchableBase>
    {
        public DeleteByTypeSpecification(DeleteByTypeQuery query)
        {
            Descriptor = new DeleteByQueryDescriptor<SearchableBase>()
                //.AllIndices()
                .Query(q =>
                    q.Term(t =>
                        t.Field(v => v.Type)
                            .Value(query.Type.ToInt())));
        }
    }
}