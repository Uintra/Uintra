using System.Collections.Generic;

namespace uCommunity.Core.App_Plugins.Core.Controls.FileUpload.Core
{
    public class FileUploadEditViewModel
    {
        public FileUploadSettins Settings { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}