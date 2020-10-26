
namespace Uintra20.Features.Search.Models
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