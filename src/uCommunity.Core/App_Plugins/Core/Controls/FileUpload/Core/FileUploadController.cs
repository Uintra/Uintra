using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.Extentions;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Core.App_Plugins.Core.Controls.FileUpload.Core
{
    public class FileUploadController : SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;

        public FileUploadController(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public ActionResult Uploader(FileUploadSettins settings)
        {
            return View("~/App_Plugins/Core/Controls/FileUpload/FileUploadView.cshtml", settings);
        }

        public ActionResult Editor(FileUploadSettins settings, string model)
        {
            var mediaIds = model.ToIntCollection();
            var media = _umbracoHelper.TypedMedia(mediaIds);
            var files = media.Select(s => new FileViewModel
            {
                Id = s.Id,
                Url = s.GetCropUrl(UmbracoAliases.GalleryPreviewImageCrop),
                Extention = s.GetMediaExtention(),
                Type = s.GetMediaType()
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