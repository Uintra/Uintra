
namespace Compent.Uintra.Core.Search.Entities
{
    public class SearchScoreModel
    {
        public double UserNameScore { get; set; }

        public double UserEmailScore { get; set; }

        public double TitleScore { get; set; }

        public double PhoneScore { get; set; }

        public double DepartmentScore { get; set; }
    }
}