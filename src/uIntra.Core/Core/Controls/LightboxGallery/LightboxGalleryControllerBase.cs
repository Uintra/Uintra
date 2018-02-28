using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using Uintra.Core.Constants;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Media;
using UIntra.Core.Controls.LightboxGallery;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Controls.LightboxGallery
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
            var galleryItems = medias.Select(MapToMedia).OrderBy(s => s.Type).ToList();

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
            model.DisplayedImagesCount = HttpContext.Request.IsMobileBrowser() ? 2 : 3;

            galleryPreviewModel.Links = _linkService.GetLinks(model.ActivityId);
            galleryPreviewModel.Medias = FindMedias(galleryViewModelList);
            galleryPreviewModel.OtherFiles = galleryViewModelList.Except(galleryPreviewModel.Medias);
            galleryPreviewModel.Medias.Skip(model.DisplayedImagesCount).ToList().ForEach(i => i.IsHidden = true);
            galleryPreviewModel.HiddenImagesCount = galleryPreviewModel.Medias.Count(i => i.IsHidden);


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

            if (result.Type is MediaTypeEnum.Image)
            {
                result.Height = media.GetPropertyValue<int>(UmbracoAliases.Media.MediaHeight);
                result.Width = media.GetPropertyValue<int>(UmbracoAliases.Media.MediaWidth);
                return result;
            }
            if (result.Type is MediaTypeEnum.Video)
            {
                result.PreviewUrl = media.GetPropertyValue<string>(UmbracoAliases.Video.ThumbnailUrlPropertyAlias);
                result.Height = media.GetPropertyValue<int>(UmbracoAliases.Video.VideoHeightPropertyAlias);
                result.Width = media.GetPropertyValue<int>(UmbracoAliases.Video.VideoWidthPropertyAlias);
                return result;
            }

            return result;
        }

        protected void MapPreviewUrl(List<LightboxGalleryItemViewModel> galleryItems)
        {
            var imageItems = galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image);

            if (imageItems.Count == 1)
            {
                var item = imageItems[0];
                item.PreviewUrl = _imageHelper.GetImageWithPreset(item.Url, IsPortrait(item.Width, item.Height) ? UmbracoAliases.ImagePresets.CroppedPreview : UmbracoAliases.ImagePresets.Preview);
                return;
            }

            foreach (var item in imageItems)
            {
                item.PreviewUrl = imageItems.Count < 3 ?
                    _imageHelper.GetImageWithPreset(item.Url, IsPortrait(item.Width, item.Height) ? UmbracoAliases.ImagePresets.CroppedPreviewTwo : UmbracoAliases.ImagePresets.PreviewTwo) :
                    _imageHelper.GetImageWithPreset(item.Url, IsPortrait(item.Width, item.Height) ? UmbracoAliases.ImagePresets.CroppedThumbnail : UmbracoAliases.ImagePresets.Thumbnail);
            }
        }

        private bool IsPortrait(int width, int height)
        {
            var isPortrait = height - width > 1.1;
            return isPortrait;
        }

        protected List<LightboxGalleryItemViewModel> FindMedias(List<LightboxGalleryItemViewModel> galleryItems)
        {
            return galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image || m.Type is MediaTypeEnum.Video);
        }
    }
}