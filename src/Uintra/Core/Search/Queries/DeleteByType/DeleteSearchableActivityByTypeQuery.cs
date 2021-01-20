using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries.DeleteByType
{
    public class DeleteSearchableActivityByTypeQuery : ISearchQuery<SearchableActivity>
    {
        public UintraSearchableTypeEnum Type { get; set; }
    }
}