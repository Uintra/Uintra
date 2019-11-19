namespace Uintra20.Features.CentralFeed.Models.GroupFeed
{
    public struct GroupInfo
    {
        public string Title { get; }
        public string Url { get; }

        public GroupInfo(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }
}