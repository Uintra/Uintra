using Compent.Shared.Search.Contract;
using System.Threading.Tasks;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Repository
{
    public interface IUintraSearchRepository<T> : ISearchRepository<T> where T : ISearchDocument
    {
        Task<bool> DeleteByType(UintraSearchableTypeEnum type);
    }
    public interface IUintraSearchRepository : ISearchRepository
    {
        Task<Entities.SearchResult<SearchableBase>> SearchAsyncTyped<TQuery>(TQuery query)
            where TQuery : ISearchQuery<SearchDocument>;
    }
}