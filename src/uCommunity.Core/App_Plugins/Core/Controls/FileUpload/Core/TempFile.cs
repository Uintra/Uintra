using System;

namespace uCommunity.Core.App_Plugins.Core.Controls.FileUpload.Core
{
    public class TempFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}