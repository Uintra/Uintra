using System.Configuration;
using System.Globalization;
using Compent.Uintra.Core.Search.Entities;

namespace Compent.Uintra.Core.Search
{
    public class SearchScoreProvider : ISearchScoreProvider
    {
        private const string UserNameScoreKey = "Search.UserNameScore";
        private const string UserEmailScoreKey = "Search.UserEmailScore";
        private const string TitleScoreKey = "Search.TitleScore";

        public SearchScoreModel GetScores()
        {
            var scores = new SearchScoreModel()
            {
                UserNameScore = double.Parse(ConfigurationManager.AppSettings[UserNameScoreKey], CultureInfo.InvariantCulture),
                UserEmailScore = double.Parse(ConfigurationManager.AppSettings[UserEmailScoreKey], CultureInfo.InvariantCulture),
                TitleScore = double.Parse(ConfigurationManager.AppSettings[TitleScoreKey], CultureInfo.InvariantCulture)
            };

            return scores;
        }
    }
}