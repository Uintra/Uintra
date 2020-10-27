using Uintra.Core.Controls.FileUpload;

namespace Uintra.Features.Media.Models
{
    public class MediaConvertModel
    {
        public int MediaId { get; set; }

        public TempFile File { get; set; }
    }
}