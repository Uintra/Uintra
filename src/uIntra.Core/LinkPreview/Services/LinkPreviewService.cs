using uIntra.Core.Extensions;
using uIntra.Core.Utils;

namespace uIntra.Core.LinkPreview.Services
{
    public class ILinkPreviewService
    {
        private readonly IHttpHelper _httpHelper;

        public ILinkPreviewService(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public LinkPreviewDto GetLinkPreview(string url)
        {
            return _httpHelper.GetStringAsync($"http://localhost:62224/api/preview?url={url}").Result.Deserialize<LinkPreviewDto>();
        }
    }
}
