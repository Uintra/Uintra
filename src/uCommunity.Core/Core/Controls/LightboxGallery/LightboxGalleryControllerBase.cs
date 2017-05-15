using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Extentions;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Core.Controls.LightboxGallery
{
    public  abstract class LightboxGalleryControllerBase: SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;

        protected LightboxGalleryControllerBase(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public virtual ActionResult RenderGallery(string mediaIds)
        {
            var result = Enumerable.Empty<LightboxGalleryViewModel>();

            if (mediaIds.IsNotNullOrEmpty())
            {
                var ids = mediaIds.ToIntCollection();
                var medias = _umbracoHelper.TypedMedia(ids).ToList();
                result = medias.Map<IEnumerable<LightboxGalleryViewModel>>().OrderBy(s => s.Type);
            }

            return View("~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGallery.cshtml", result);
        }

        
        public virtual ActionResult Preview(LightboxGalleryPreviewModel model)
        {
            var galleryPreviewModel = new LightboxGalleryPreviewViewModel();
            if (model.MediaIds.Any())
            {
                var galleryViewModelList = _umbracoHelper.TypedMedia(model.MediaIds).Map<List<LightboxGalleryViewModel>>();
                galleryPreviewModel.Images = galleryViewModelList.Where(m => m.Type == MediaTypeEnum.Image);
                galleryPreviewModel.OtherFiles = galleryViewModelList.Except(galleryPreviewModel.Images);
                galleryPreviewModel.Url = $"{model.Url}#{GetOverviewElementId()}";
            }

            return View("~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGalleryPreview.cshtml", galleryPreviewModel);
        }

        protected virtual string GetOverviewElementId()
        {
            return "js-lightbox-gallery";
        }
    }
}