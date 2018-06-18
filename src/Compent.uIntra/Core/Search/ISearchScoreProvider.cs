using Compent.uIntra.Core.Search.Entities;

namespace Compent.uIntra.Core.Search
{
    public interface ISearchScoreProvider
    {
        SearchScoreModel GetScores();
    }
}