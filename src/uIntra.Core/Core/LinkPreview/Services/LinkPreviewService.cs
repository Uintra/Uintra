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
            var dto = _httpHelper
                .GetStringAsync($"http://localhost:62224/api/preview?url={url}")
                .Result
                .Deserialize<LinkPreviewDto>();

            var result = dto.Map<LinkPreview>();

            result.Description = GetLongest(dto.OgDescription, dto.Description);

            return result;
        }

        private static string GetLongest(string first, string second) =>
            GetNullableLenght(first) > GetNullableLenght(second) ? first : second;

        private static int GetNullableLenght(string str) =>
            str?.Length ?? default;
    }
}
