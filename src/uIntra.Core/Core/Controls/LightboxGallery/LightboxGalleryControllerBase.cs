using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Controls.LightboxGallery
{
    public abstract class LightboxGalleryControllerBase : SurfaceController
    {
        protected virtual string GalleryViewPath { get; } = "~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGallery.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Core/Controls/LightBoxGallery/LightboxGalleryPreview.cshtml";

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

            return View(GalleryViewPath, result);
        }


        public virtual ActionResult Preview(LightboxGalleryPreviewModel model)
        {
            if (model.MediaIds.IsEmpty())
            {
                return new EmptyResult();
            }

            var galleryPreviewModel = model.Map<LightboxGalleryPreviewViewModel>();
            var galleryViewModelList = _umbracoHelper.TypedMedia(model.MediaIds).Map<List<LightboxGalleryViewModel>>();

            if (galleryViewModelList.Count == 0)
            {
                return new EmptyResult();
            }

            galleryPreviewModel.Images = galleryViewModelList.FindAll(m => m.Type.Id == MediaTypeEnum.Image.ToInt());
            galleryPreviewModel.OtherFiles = galleryViewModelList.FindAll(m => m.Type.Id != MediaTypeEnum.Image.ToInt());
            galleryPreviewModel.Images.Skip(model.DisplayedImagesCount).ForEach(i => i.IsHidden = true);

            return View(PreviewViewPath, galleryPreviewModel);
        }
    }
}