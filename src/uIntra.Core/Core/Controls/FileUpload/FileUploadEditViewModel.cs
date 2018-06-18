using System.Collections.Generic;

namespace Uintra.Core.Controls.FileUpload
{
    public class FileUploadEditViewModel
    {
        public FileUploadSettings Settings { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}