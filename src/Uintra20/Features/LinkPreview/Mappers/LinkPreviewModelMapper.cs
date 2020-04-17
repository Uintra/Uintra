using System;
using Compent.LinkPreview.HttpClient;
using Uintra20.Features.LinkPreview.Sql;

namespace Uintra20.Features.LinkPreview.Mappers
{
    public class LinkPreviewModelMapper
    {
        private const string DefaultPreviewImagePath = "/content/images/preview.png";
        private readonly ILinkPreviewUriProvider _linkPreviewUriProvider;

        public LinkPreviewModelMapper(
            ILinkPreviewUriProvider linkPreviewUriProvider)
        {
            _linkPreviewUriProvider = linkPreviewUriProvider;
        }

        public Models.LinkPreview MapPreview(LinkPreviewEntity entity)
        {
            var result = new Models.LinkPreview
            {
                Id = entity.Id,
                Uri = new UriBuilder(entity.Uri).Uri,
                Title = entity.Title,
                Description = GetLongest(entity.OgDescription, entity.Description)
            };

            if (entity.MediaId.HasValue)
            {
                var media = Umbraco.Web.Composing.Current.UmbracoHelper.Media(entity.MediaId);
                result.ImageUri = media != null ? new Uri(media.Url, UriKind.Relative) : null;
            }
            else
            {
                result.ImageUri = entity.ImageId.HasValue ? _linkPreviewUriProvider.GetImageUri(entity.ImageId.Value) : null;
                result.FaviconUri = entity.FaviconId.HasValue ? _linkPreviewUriProvider.GetImageUri(entity.FaviconId.Value) : null;
            }

            if (result.ImageUri == null)
            {
                result.ImageUri = new Uri(DefaultPreviewImagePath, UriKind.Relative);
            }
            return result;
        }

        private static string GetLongest(string first, string second) =>
            GetNullableLength(first) > GetNullableLength(second) ? first : second;

        private static int GetNullableLength(string str) =>
            str?.Length ?? 0;
    }
}