using Compent.Shared.Search.Elasticsearch;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteSearchableActivityByTypeSpecification : DeleteQuerySpecification<SearchableActivity>
    {
        public DeleteSearchableActivityByTypeSpecification(DeleteSearchableActivityByTypeQuery query)
        {
            Descriptor = new DeleteByQueryDescriptor<SearchableActivity>()
                //.AllIndices()
                .Query(q =>
                    q.Term(t =>
                        t.Field(v => v.Type)
                            .Value(query.Type.ToInt())));
        }
    }
}