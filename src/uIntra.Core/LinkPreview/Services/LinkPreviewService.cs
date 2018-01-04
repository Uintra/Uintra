using uIntra.Core.Extensions;
using uIntra.Core.Utils;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewService : ILinkPreviewService
    {
        private readonly IHttpHelper _httpHelper;

        public LinkPreviewService(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public LinkPreview GetLinkPreview(string url)
        {
            return _httpHelper.GetStringAsync($"http://localhost:62224/api/preview?url={url}").Result.Deserialize<LinkPreview>();
        }
    }
}
