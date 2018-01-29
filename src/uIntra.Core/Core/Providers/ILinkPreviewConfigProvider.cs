namespace uIntra.Core
{
    public interface ILinkPreviewConfigProvider
    {
        FooBananaConfig Config { get; }
    }

    public class FooBananaConfig
    {
        public string UrlRegex { get; }

        public FooBananaConfig(string urlRegex)
        {
            UrlRegex = urlRegex;
        }
    }
}
