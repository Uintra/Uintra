using Uintra.Features.Links.Models;

namespace Uintra.Features.CentralFeed.Models.GroupFeed
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