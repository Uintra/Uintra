using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Social.Models;

namespace Uintra20.Core.HomePage
{
    public class HomePageViewModel : UBaseline.Shared.HomePage.HomePageViewModel
    {
        public UintraLinkModel UserListPage { get; set; }
        public UintraLinkModel CreateNewsLink { get; set; }
        public UintraLinkModel CreateEventsLink { get; set; }
        public SocialCreatePageViewModel SocialCreateModel { get; set; }
    }
}