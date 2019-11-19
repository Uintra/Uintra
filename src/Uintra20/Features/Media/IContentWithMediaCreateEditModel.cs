namespace Uintra20.Features.Media
{
    public interface IContentWithMediaCreateEditModel
    {
        int? MediaRootId { get; set; }
        string NewMedia { get; set; }
    }
}