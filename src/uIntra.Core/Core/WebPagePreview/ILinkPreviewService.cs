namespace uIntra.Core.WebPagePreview
{
    public interface ILinkPreviewService
    {
        byte[] GetHtmlPreviewByteArray(string url);
    }
}
