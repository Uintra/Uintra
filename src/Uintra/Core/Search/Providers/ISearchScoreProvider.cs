using Uintra.Features.Search.Models;

namespace Uintra.Core.Search.Providers
{
    public interface ISearchScoreProvider
    {
        SearchScoreModel GetScores();
    }
}