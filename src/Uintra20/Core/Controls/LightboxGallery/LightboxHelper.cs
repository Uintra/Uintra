using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Controls.LightboxGallery
{
    public class LightboxHelper : ILightboxHelper
    {
        private readonly UmbracoHelper _helper;
        private readonly IImageHelper _imageHelper;

        public LightboxHelper(UmbracoHelper helper, IImageHelper imageHelper)
        {
            _helper = helper;
            _imageHelper = imageHelper;
        }

        public void FillGalleryPreview(IHaveLightboxPreview model, IEnumerable<int> mediaIds)
        {
            model.MediaPreview = GetGalleryPreviewModel(mediaIds);
        }

        public LightboxPreviewModel GetGalleryPreviewModel(IEnumerable<int> mediaIds)
        {
            var medias = _helper.Media(mediaIds);

            var mediasList = medias.ToList();
            var galleryViewModelList = mediasList.Select(MapToMedia).ToList();

            TransformPreviewImage(galleryViewModelList);
            var displayedImagesCount = HttpContext.Current.Request.IsMobileBrowser() ? 2 : 3;

            List<LightboxGalleryItemViewModel> mediasViewModel = FindMedias(galleryViewModelList);

            var galleryPreviewModel = new LightboxPreviewModel
            {
                Medias = mediasViewModel.Map<IEnumerable<LightboxGalleryItemPreviewModel>>(),
                OtherFiles = galleryViewModelList.Except(mediasViewModel)
                    .Map<IEnumerable<LightboxGalleryItemPreviewModel>>()
            }; 
            galleryPreviewModel.Medias.Skip(displayedImagesCount).ToList().ForEach(i => i.IsHidden = true);
            galleryPreviewModel.HiddenImagesCount = galleryPreviewModel.Medias.Count(i => i.IsHidden);
            
            return galleryPreviewModel;
        }

        private LightboxGalleryItemViewModel MapToMedia(IPublishedContent media)
        {
            var result = new LightboxGalleryItemViewModel
            {
                Id = media.Id,
                Url = media.Url,
                Name = media.Name,
                Extension = media.GetMediaExtension(),
                Type = media.GetMediaType()
            };

            if (result.Type is MediaTypeEnum.Image)
            {
                result.Height = media.Value<int>(UmbracoAliases.Media.MediaHeight);
                result.Width = media.Value<int>(UmbracoAliases.Media.MediaWidth);
                return result;
            }
            if (result.Type is MediaTypeEnum.Video)
            {
                result.PreviewUrl = media.Value<string>(UmbracoAliases.Video.ThumbnailUrlPropertyAlias);
                result.Height = media.Value<int>(UmbracoAliases.Video.VideoHeightPropertyAlias);
                result.Width = media.Value<int>(UmbracoAliases.Video.VideoWidthPropertyAlias);
                return result;
            }

            return result;
        }

        private void TransformPreviewImage(List<LightboxGalleryItemViewModel> galleryItems)
        {
            var imageItems = galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image || m.Type is MediaTypeEnum.Video);

            if (imageItems.Count == 1)
            {
                var item = imageItems[0];

                item.PreviewUrl = _imageHelper.GetImageWithResize(IsVideo(item.Type) ? item.PreviewUrl : item.Url, UmbracoAliases.ImageResize.Preview);

                return;
            }

            foreach (var item in imageItems)
            {
                item.PreviewUrl = imageItems.Count < 3 ?
                    _imageHelper.GetImageWithResize(IsVideo(item.Type) ? item.PreviewUrl : item.Url, UmbracoAliases.ImageResize.PreviewTwo) :
                    _imageHelper.GetImageWithResize(IsVideo(item.Type) ? item.PreviewUrl : item.Url, UmbracoAliases.ImageResize.Thumbnail);
            }
        }

        private List<LightboxGalleryItemViewModel> FindMedias(List<LightboxGalleryItemViewModel> galleryItems)
        {
            return galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image || m.Type is MediaTypeEnum.Video);
        }

        private bool IsVideo(Enum type)
        {
            return type.ToInt() == MediaTypeEnum.Video.ToInt();
        }
    }
}