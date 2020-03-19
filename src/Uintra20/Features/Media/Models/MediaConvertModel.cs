using Uintra20.Core.Controls.FileUpload;

namespace Uintra20.Features.Media.Models
{
    public class MediaConvertModel
    {
        public int MediaId { get; set; }

        public TempFile File { get; set; }
    }
}