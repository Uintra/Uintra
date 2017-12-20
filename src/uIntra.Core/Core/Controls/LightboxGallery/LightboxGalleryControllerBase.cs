using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        private readonly ImageHelper _imageHelper;

        protected LightboxGalleryControllerBase(UmbracoHelper umbracoHelper, IActivityLinkService linkService, ImageHelper imageHelper)
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
            if (model.MediaIds.IsEmpty())
            {
                return new EmptyResult();
            }

            var medias = _umbracoHelper.TypedMedia(model.MediaIds).ToList();
            if (medias.IsEmpty())
            {
                return new EmptyResult();
            }

            var galleryPreviewModel = GetGalleryPreviewModel(model, medias);
            return PartialView(PreviewViewPath, galleryPreviewModel);
        }

        protected virtual LightboxGalleryOverviewModel GetGalleryOverviewModel(string mediaIds)
        {
            var result = new LightboxGalleryOverviewModel();
            if (mediaIds.IsNullOrEmpty())
            {
                return result;
            }

            var ids = mediaIds.ToIntCollection();
            var medias = _umbracoHelper.TypedMedia(ids).ToList();
            result.GalleryItems = medias.Select(m => MapToMedia(m, medias.Count)).OrderBy(s => s.Type.Id);

            return result;
        }

        protected virtual LightboxGalleryItemViewModel MapToMedia(IPublishedContent media, int totalMediasCount)
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
                result.PreviewUrl = totalMediasCount == 1 ? _imageHelper.ToPreviewImage(media.Url) : _imageHelper.ToThumbnailImage(media.Url);
            }

            return result;
        }

        protected virtual LightboxGalleryPreviewViewModel GetGalleryPreviewModel(LightboxGalleryPreviewModel model, IEnumerable<IPublishedContent> medias)
        {
            var galleryPreviewModel = model.Map<LightboxGalleryPreviewViewModel>();

            var totalMediasCount = medias.Count();
            var galleryViewModelList = medias.Select(m => MapToMedia(m, totalMediasCount)).ToList();

            galleryPreviewModel.Links = _linkService.GetLinks(model.ActivityId);
            galleryPreviewModel.Images = galleryViewModelList.FindAll(m => m.Type.Id == MediaTypeEnum.Image.ToInt());
            galleryPreviewModel.OtherFiles = galleryViewModelList.FindAll(m => m.Type.Id != MediaTypeEnum.Image.ToInt());
            galleryPreviewModel.Images.Skip(model.DisplayedImagesCount).ToList().ForEach(i => i.IsHidden = true);

            return galleryPreviewModel;
        }
    }
}