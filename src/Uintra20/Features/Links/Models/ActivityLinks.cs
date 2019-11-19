namespace Uintra20.Features.Links.Models
{
    public class ActivityLinks : ActivityCreateLinks, IActivityLinks
    {
        public string Details { get; set; }
        public string Edit { get; set; }
    }
}