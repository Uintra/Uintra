using Uintra20.Features.Search.Models;

namespace Uintra20.Features.Search
{
    public interface ISearchScoreProvider
    {
        SearchScoreModel GetScores();
    }
}