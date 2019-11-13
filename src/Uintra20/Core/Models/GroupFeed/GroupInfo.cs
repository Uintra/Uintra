namespace Uintra20.Core.Groups
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