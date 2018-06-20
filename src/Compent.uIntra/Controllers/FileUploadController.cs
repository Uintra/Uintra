using Uintra.Core.Controls.FileUpload;
using Uintra.Core.Media;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class FileUploadController : FileUploadControllerBase
    {
        public FileUploadController(UmbracoHelper umbracoHelper, IImageHelper imageHelper)
            : base(umbracoHelper, imageHelper)
        {
        }
    }
}