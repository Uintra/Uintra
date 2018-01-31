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
            var galleryItems = medias.Select(MapToMedia).OrderBy(s => s.Type).ToList();

            MapPreviewUrl(galleryItems);
            result.GalleryItems = galleryItems;
            return result;
        }

        protected virtual LightboxGalleryPreviewViewModel GetGalleryPreviewModel(LightboxGalleryPreviewModel model, IEnumerable<IPublishedContent> medias)
        {
            var galleryPreviewModel = model.Map<LightboxGalleryPreviewViewModel>();
            var mediasList = medias.AsList();
            var galleryViewModelList = mediasList.Select(MapToMedia).ToList();
            MapPreviewUrl(galleryViewModelList);

            galleryPreviewModel.Links = _linkService.GetLinks(model.ActivityId);
            galleryPreviewModel.Images = galleryViewModelList.FindAll(m => m.Type is  MediaTypeEnum.Image);
            galleryPreviewModel.OtherFiles = galleryViewModelList.FindAll(m => m.Type is MediaTypeEnum.Image);
            galleryPreviewModel.Images.Skip(model.DisplayedImagesCount).ToList().ForEach(i => i.IsHidden = true);

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
            }

            return result;
        }

        protected void MapPreviewUrl(List<LightboxGalleryItemViewModel> galleryItems)
        {
            var imageItems = galleryItems.FindAll(m => m.Type is MediaTypeEnum.Image);

            foreach (var item in imageItems)
            {
                item.PreviewUrl = imageItems.Count > 1 ?
                    _imageHelper.GetImageWithPreset(item.Url, UmbracoAliases.ImagePresets.Thumbnail) :
                    _imageHelper.GetImageWithPreset(item.Url, UmbracoAliases.ImagePresets.Preview);
            }
        }
    }
}