namespace uCommunity.Core.Media
{
    public interface IContentWithMediaCreateEditModel
    {
        int? MediaRootId { get; set; }
        string NewMedia { get; set; }
    }
}