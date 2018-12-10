using Umbraco.Core.Models;

namespace Uintra.Navigation
{
    public class UserListLinkModel
    {
        public IPublishedContent ContentPage { get; set; }
        public bool IsVisible { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
