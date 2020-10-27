using System;
using System.Web;
using Compent.LinkPreview.HttpClient;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.LinkPreview.Sql;

namespace Uintra.Features.LinkPreview.Mappers
{
	public class LinkPreviewModelMapper
	{
		private const string DefaultPreviewImagePath = "/images/preview.png";
		private readonly ILinkPreviewUriProvider _linkPreviewUriProvider;

		public LinkPreviewModelMapper(
			ILinkPreviewUriProvider linkPreviewUriProvider)
		{
			_linkPreviewUriProvider = linkPreviewUriProvider;
		}

		public LinkPreviewModel MapPreview(LinkPreviewEntity entity)
		{
			var result = new LinkPreviewModel
			{
				Id = entity.Id,
				Uri = new UriBuilder(entity.Uri).Uri.AbsoluteUri,
				Title = HttpUtility.HtmlDecode(entity.Title),
				Description = HttpUtility.HtmlDecode(GetLongest(entity.OgDescription, entity.Description))
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