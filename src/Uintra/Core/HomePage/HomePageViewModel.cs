using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra.Features.Links.Models;
using Uintra.Features.Social.Models;

namespace Uintra.Core.HomePage
{
    public class HomePageViewModel : UBaseline.Shared.HomePage.HomePageViewModel
    {
        public UintraLinkModel UserListPage { get; set; }
        public UintraLinkModel CreateNewsLink { get; set; }
        public UintraLinkModel CreateEventsLink { get; set; }
        public SocialCreatePageViewModel SocialCreateModel { get; set; }
    }
}