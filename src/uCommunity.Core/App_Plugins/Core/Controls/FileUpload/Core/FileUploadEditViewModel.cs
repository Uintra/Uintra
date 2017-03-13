using System.Collections.Generic;

namespace uCommunity.Core.Controls.FileUpload
{
    public class FileUploadEditViewModel
    {
        public FileUploadSettins Settings { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}