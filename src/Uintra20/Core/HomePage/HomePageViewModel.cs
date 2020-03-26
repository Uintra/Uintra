using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Social.Models;

namespace Uintra20.Core.HomePage
{
    public class HomePageViewModel : INodeViewModel
    {
        public int Id { get; set; }
        public string ContentTypeAlias { get; set; }
        public string Name { get; set; }
        public bool AddToSitemap { get; set; }
        public string Url { get; set; }
        public NodeType NodeType { get; set; }
        public UintraLinkModel UserListPage { get; set; }
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public UintraLinkModel CreateNewsLink { get; set; }
        public UintraLinkModel CreateEventsLink { get; set; }
        public SocialCreatePageViewModel SocialCreateModel { get; set; }
    }
}