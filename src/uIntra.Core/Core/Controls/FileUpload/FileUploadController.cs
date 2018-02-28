using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Constants;
using Uintra.Core.Extensions;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Controls.FileUpload
{
    public class FileUploadController : SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;

        public FileUploadController(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public ActionResult Uploader(FileUploadSettings settings)
        {
            return View("~/App_Plugins/Core/Controls/FileUpload/FileUploadView.cshtml", settings);
        }

        public ActionResult Editor(FileUploadSettings settings, string model)
        {
            var mediaIds = model.ToIntCollection();
            var media = _umbracoHelper.TypedMedia(mediaIds);
            var files = media.Select(s => new FileViewModel
            {
                Id = s.Id,
                Url = s.GetCropUrl(UmbracoAliases.GalleryPreviewImageCrop),
                Extension = s.GetMediaExtension(),
                Type = s.GetMediaType(),
                FileName = s.Name
            });

            var viewModel = new FileUploadEditViewModel
            {
                Settings = settings,
                Files = files
            };

            return View("~/App_Plugins/Core/Controls/FileUpload/FileUploadEditView.cshtml", viewModel);
        }
    }
}