namespace uCommunity.Core.App_Plugins.Core.Media
{
    public interface IContentWithMediaCreateEditModel
    {
        int? MediaRootId { get; set; }
        string NewMedia { get; set; }
    }
}