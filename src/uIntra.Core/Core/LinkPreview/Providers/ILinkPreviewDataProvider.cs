namespace uIntra.Core.LinkPreview
{
    public interface ILinkPreviewDataProvider
    {
        LinkPreviewDto GetLinkPreviewDto(string link);
    }
}