using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Navigation.Models
{
    public class UserListLinkViewModel
    {
        public IPublishedContent ContentPage { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}