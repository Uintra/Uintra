using uIntra.Core.Extensions;
using uIntra.Core.Utils;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewDataProvider : ILinkPreviewDataProvider
    {
        private readonly IHttpHelper _httpHelper;
        private readonly ILinkPreviewConfiguration _configuration;

        public LinkPreviewDataProvider(IHttpHelper httpHelper, ILinkPreviewConfiguration configuration)
        {
            _httpHelper = httpHelper;
            _configuration = configuration;
        }

        public LinkPreviewDto GetLinkPreviewDto(string link) =>
            _httpHelper
                .GetStringAsync(GetRequestUri(link))
                .Result
                .Deserialize<LinkPreviewDto>();

        private string GetRequestUri(string link) => 
            $"{_configuration.LinkPreviewServiceUrl}/api/preview?url={link}";
    }
}
