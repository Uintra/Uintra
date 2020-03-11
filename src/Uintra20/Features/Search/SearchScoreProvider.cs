using System.Configuration;
using System.Globalization;
using Uintra20.Features.Search.Models;

namespace Uintra20.Features.Search
{
    public class SearchScoreProvider : ISearchScoreProvider
    {
        private const string UserNameScoreKey = "Search.UserNameScore";
        private const string UserEmailScoreKey = "Search.UserEmailScore";
        private const string TitleScoreKey = "Search.TitleScore";
        private const string PhoneScoreKey = "Search.UserPhoneScore";
        private const string DepartmentScoreKey = "Search.UserDepartmentScore";

        public SearchScoreModel GetScores()
        {
            var scores = new SearchScoreModel()
            {
                UserNameScore = double.Parse(ConfigurationManager.AppSettings[UserNameScoreKey], CultureInfo.InvariantCulture),
                UserEmailScore = double.Parse(ConfigurationManager.AppSettings[UserEmailScoreKey], CultureInfo.InvariantCulture),
                TitleScore = double.Parse(ConfigurationManager.AppSettings[TitleScoreKey], CultureInfo.InvariantCulture),
                PhoneScore = double.Parse(ConfigurationManager.AppSettings[PhoneScoreKey], CultureInfo.InvariantCulture),
                DepartmentScore = double.Parse(ConfigurationManager.AppSettings[DepartmentScoreKey], CultureInfo.InvariantCulture)
            };

            return scores;
        }
    }
}