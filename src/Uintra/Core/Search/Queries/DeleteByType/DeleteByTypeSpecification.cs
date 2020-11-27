using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Queries
{
    public class DeleteByTypeSpecification : DeleteQuerySpecification<SearchableBase>
    {
        public DeleteByTypeSpecification(DeleteByTypeQuery<SearchableBase> query)
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