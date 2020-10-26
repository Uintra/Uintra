using Uintra20.Features.Search.Models;

namespace Uintra20.Core.Search.Providers
{
    public interface ISearchScoreProvider
    {
        SearchScoreModel GetScores();
    }
}