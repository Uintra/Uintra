using System;
using Compent.LinkPreview.HttpClient;
using Uintra.Core.LinkPreview.Sql;
using Umbraco.Web;

namespace Uintra.Core.LinkPreview
{
    public class LinkPreviewModelMapper
    {
        private const string defaultPreviewImagePath = "/content/images/preview.png";
        private readonly ILinkPreviewUriProvider _linkPreviewUriProvider;
        private readonly UmbracoHelper _umbracoHelper;

        public LinkPreviewModelMapper(
            ILinkPreviewUriProvider linkPreviewUriProvider,
            UmbracoHelper umbracoHelper)
        {
            _linkPreviewUriProvider = linkPreviewUriProvider;
            _umbracoHelper = umbracoHelper;
        }

        public LinkPreview MapPreview(LinkPreviewEntity entity)
        {
            var result = new LinkPreview
            {
                Id = entity.Id,
                Uri = new UriBuilder(entity.Uri).Uri,
                Title = entity.Title,
                Description = GetLongest(entity.OgDescription, entity.Description)
            };

            if (entity.MediaId.HasValue)
            {
                var media = _umbracoHelper.TypedMedia(entity.MediaId);
                result.ImageUri = media != null ? new Uri(media.Url, UriKind.Relative) : null;
            }
            else
            {
                result.ImageUri = entity.ImageId.HasValue ? _linkPreviewUriProvider.GetImageUri(entity.ImageId.Value) : null;
                result.FaviconUri = entity.FaviconId.HasValue ? _linkPreviewUriProvider.GetImageUri(entity.FaviconId.Value) : null;
            }

            if (result.ImageUri == null)
            {
                result.ImageUri = new Uri(defaultPreviewImagePath, UriKind.Relative);
            }
            return result;
        }

        private static string GetLongest(string first, string second) =>
            GetNullableLength(first) > GetNullableLength(second) ? first : second;

        private static int GetNullableLength(string str) =>
            str?.Length ?? default;
    }
}
