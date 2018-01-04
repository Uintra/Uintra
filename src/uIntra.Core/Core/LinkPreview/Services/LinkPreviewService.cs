using uIntra.Core.Extensions;
using uIntra.Core.Utils;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewService : ILinkPreviewService
    {
        private readonly ILinkPreviewDataProvider _dataProvider;

        public LinkPreviewService(ILinkPreviewDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public LinkPreview GetLinkPreview(string link)
        {
            var dto = _dataProvider.GetLinkPreviewDto(link);

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
