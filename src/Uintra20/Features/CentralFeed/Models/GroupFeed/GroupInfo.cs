using Uintra20.Features.Links.Models;

namespace Uintra20.Features.CentralFeed.Models.GroupFeed
{
    public struct GroupInfo
    {
        public string Title { get; }
        public UintraLinkModel Url { get; }

        public GroupInfo(string title, UintraLinkModel url)
        {
            Title = title;
            Url = url;
        }
    }
}