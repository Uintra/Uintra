using Compent.Uintra.Core.Search.Entities;

namespace Compent.Uintra.Core.Search
{
    public interface ISearchScoreProvider
    {
        SearchScoreModel GetScores();
    }
}