using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;
using Umbraco.Core.Migrations.Expressions.Update;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteByTypeQuery : ISearchQuery<SearchableBase>
    {
        public UintraSearchableTypeEnum Type { get; set; }
    }
}