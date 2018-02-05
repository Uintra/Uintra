using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Controls.LightboxGallery
{
    public abstract class LightboxGalleryControllerBase : SurfaceController
    {
        protected virtual string GalleryViewPath { get; } = "~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGallery.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGalleryPreview.cshtml";

        private readonly UmbracoHelper _umbracoHelper;
        private readonly IActivityLinkService _linkService;
        private readonly IImageHelper _imageHelper;

        protected LightboxGalleryControllerBase(UmbracoHelper umbracoHelper, IActivityLinkService linkService, IImageHelper imageHelper)
        {
            _umbracoHelper = umbracoHelper;
            _linkService = linkService;
            _imageHelper = imageHelper;
        }

        public virtual ActionResult RenderGallery(string mediaIds)
        {
            var model = GetGalleryOverviewModel(mediaIds);
            return View(GalleryViewPath, model);
        }

        public virtual ActionResult Preview(LightboxGalleryPreviewModel model)
        {
            if (model.MediaIds.IsEmpty()) return new EmptyResult();

            var medias = _umbracoHelper.TypedMedia(model.MediaIds).ToList();
            if (medias.IsEmpty()) return new EmptyResult();

            var galleryPreviewModel = GetGalleryPreviewModel(model, medias);
            return PartialView(PreviewViewPath, galleryPreviewModel);
        }

        protected virtual LightboxGalleryOverviewModel GetGalleryOverviewModel(string mediaIds)
        {
            var result = new LightboxGalleryOverviewModel();
            if (mediaIds.IsNullOrEmpty()) return result;

            var medias = _umbracoHelper.TypedMedia(mediaIds.ToIntCollection()).ToList();
            var galleryItems = medias.Select(MapToMedia).ToList();

            MapPreviewUrl(galleryItems);

            result.Medias = FindMedias(galleryItems);
            result.OtherFiles = galleryItems.Except(result.Medias);

            return result;
        }

        protected virtual LightboxGalleryPreviewViewModel GetGalleryPreviewModel(LightboxGalleryPreviewModel model, IEnumerable<IPublishedContent> medias)
        {
            var galleryPreviewModel = model.Map<LightboxGalleryPreviewViewModel>();
            var mediasList = medias.AsList();
            var galleryViewModelList = mediasList.Select(MapToMedia).ToList();

            MapPreviewUrl(galleryViewModelList);

            galleryPreviewModel.Links = _linkService.GetLinks(model.ActivityId);
            galleryPreviewModel.Medias = FindMedias(galleryViewModelList);
            galleryPreviewModel.OtherFiles = galleryViewModelList.Except(galleryPreviewModel.Medias);
            galleryPreviewModel.Medias.Skip(model.DisplayedImagesCount).ToList().ForEach(i => i.IsHidden = true);

            return galleryPreviewModel;
        }

        protected virtual LightboxGalleryItemViewModel MapToMedia(IPublishedContent media)
        {
            var result = new LightboxGalleryItemViewModel
            {
                Id = media.Id,
                Url = media.Url,
                Name = media.GetFileName(),
                Extension = media.GetMediaExtension(),
                Type = media.GetMediaType()
            };

            if (result.Type.Id == MediaTypeEnum.Image.ToInt())
            {
                result.Height = media.GetPropertyValue<int>(UmbracoAliases.Media.MediaHeight);
                result.Width = media.GetPropertyValue<int>(UmbracoAliases.Media.MediaWidth);
            }

            if (result.Type.Id == MediaTypeEnum.Video.ToInt())
            {
                result.PreviewUrl = media.GetPropertyValue<string>(UmbracoAliases.Video.ThumbnailUrlPropertyAlias);
                result.Height = media.GetPropertyValue<int>(UmbracoAliases.Video.VideoHeightPropertyAlias);
                result.Width = media.GetPropertyValue<int>(UmbracoAliases.Video.VideoWidthPropertyAlias);
            }

            return result;
        }

        protected void MapPreviewUrl(List<LightboxGalleryItemViewModel> galleryItems)
        {
            var mediaItems = FindMedias(galleryItems);

            foreach (var item in mediaItems)
            {
                var url = item.Type.Id == MediaTypeEnum.Video.ToInt() ? item.PreviewUrl : item.Url;

                item.PreviewUrl = mediaItems.Count > 1 ?
                    _imageHelper.GetImageWithPreset(url, UmbracoAliases.ImagePresets.Thumbnail) :
                    _imageHelper.GetImageWithPreset(url, UmbracoAliases.ImagePresets.Preview);
            }
        }

        protected List<LightboxGalleryItemViewModel> FindMedias(List<LightboxGalleryItemViewModel> galleryItems)
        {
            return galleryItems.FindAll(m => m.Type.Id == MediaTypeEnum.Image.ToInt() || m.Type.Id == MediaTypeEnum.Video.ToInt());
        }
    }
}