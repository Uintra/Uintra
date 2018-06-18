using System;
using Compent.LinkPreview.HttpClient;
using Uintra.Core.LinkPreview.Sql;

namespace Uintra.Core.LinkPreview
{
    public class LinkPreviewModelMapper
    {
        private readonly IUriProvider _uriProvider;

        public LinkPreviewModelMapper(IUriProvider uriProvider)
        {
            _uriProvider = uriProvider;
        }

        public LinkPreview MapPreview(LinkPreviewEntity entity)
        {
            var result = new LinkPreview
            {
                Id = entity.Id,
                Uri = new Uri(entity.Uri,UriKind.RelativeOrAbsolute),
                Title = entity.Title,
                Description = GetLongest(entity.OgDescription, entity.Description),
                ImageUri = entity.ImageId.HasValue ?  _uriProvider.GetImageUri(entity.ImageId.Value) : null,
                FaviconUri = entity.FaviconId.HasValue ? _uriProvider.GetImageUri(entity.FaviconId.Value) : null
            };

            return result;
        }

        private static string GetLongest(string first, string second) =>
            GetNullableLength(first) > GetNullableLength(second) ? first : second;

        private static int GetNullableLength(string str) =>
            str?.Length ?? default;
    }
}
