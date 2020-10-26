namespace Uintra20.Core.Feed.Models
{
    public class ActivityFeedTabFiltersViewModel
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }

        public ActivityFeedTabFiltersViewModel()
        {
            
        }

        public ActivityFeedTabFiltersViewModel(string key, string title, bool isActive)
        {
            Key = key;
            Title = title;
            IsActive = isActive;
        }
    }
}