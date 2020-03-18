using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Image.Helpers.Contracts;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Controls.LightboxGallery
{
    public class LightboxHelper : ILightboxHelper
    {
	    private readonly IImageHelper _imageHelper;

        public LightboxHelper(IImageHelper imageHelper)
        {
     
            _imageHelper = imageHelper;
        }

        public void FillGalleryPreview(IHaveLightboxPreview model, IEnumerable<int> mediaIds)
        {
            model.MediaPreview = GetGalleryPreviewModel(mediaIds, PresetStrategies.ForCentralFeed);
        }

        public LightboxPreviewModel GetGalleryPreviewModel(IEnumerable<int> mediaIds, IPresetStrategy strategy)
        {
            var medias = Umbraco.Web.Composing.Current.UmbracoHelper.Media(mediaIds);

            var mediasList = medias.ToList();

            var galleryViewModelList = mediasList.Select(MapToMedia).ToList();

            TransformPreviewImage(galleryViewModelList, strategy);

            var mediasViewModel = FindMedias(galleryViewModelList);

            var galleryPreviewModel = new LightboxPreviewModel
            {
                Medias = mediasViewModel.Map<IEnumerable<LightboxGalleryItemPreviewModel>>(),
                OtherFiles = galleryViewModelList.Except(mediasViewModel)
                    .Map<IEnumerable<LightboxGalleryItemPreviewModel>>()
            };

            galleryPreviewModel.Medias = galleryPreviewModel.Medias
                .Select(MapAsHidden())
                .ToList();

            galleryPreviewModel.HiddenImagesCount = galleryPreviewModel.Medias.Count(i => i.IsHidden);
            
            galleryPreviewModel.FilesToDisplay = strategy.MediaFilesToDisplay;
            
            var count = mediasViewModel.Count;

            if (count > 2)
            {
                galleryPreviewModel.AdditionalImages = Math.Abs(strategy.MediaFilesToDisplay - count);
            }

            return galleryPreviewModel;
        }

        private Func<LightboxGalleryItemPreviewModel, LightboxGalleryItemPreviewModel> MapAsHidden() =>
            i =>
            {
                i.IsHidden = true;
                
                return i;
            };

        private LightboxGalleryItemViewModel MapToMedia(IPublishedContent media)
        {
            var result = new LightboxGalleryItemViewModel
            {
                Id = media.Id,
                Key = media.Key,
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

        private void TransformPreviewImage(List<LightboxGalleryItemViewModel> galleryItems, IPresetStrategy strategy)
        {
            var imageItems = galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image || m.Type is MediaTypeEnum.Video);

            if (imageItems.Count == 1)
            {
                var item = imageItems[0];

                item.PreviewUrl = _imageHelper.GetImageWithResize(IsVideo(item.Type)
                    ? item.PreviewUrl
                    : item.Url, strategy.PreviewPreset);

                return;
            }

            foreach (var item in imageItems)
            {
                item.PreviewUrl = imageItems.Count < 3 ?
                    _imageHelper.GetImageWithResize(IsVideo(item.Type) ? item.PreviewUrl : item.Url, strategy.PreviewTwoPreset) :
                    _imageHelper.GetImageWithResize(IsVideo(item.Type) ? item.PreviewUrl : item.Url, strategy.ThumbnailPreset);
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