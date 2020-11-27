using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries
{
    // TODO: Search. Get rid of generic, move DeleteByTYpe to general repository
    public class DeleteByTypeQuery<T> : ISearchQuery<T> where T : class, ISearchDocument
    {
        public UintraSearchableTypeEnum Type { get; set; }
    }
}