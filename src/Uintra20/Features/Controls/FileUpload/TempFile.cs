using System;

namespace Uintra20.Features.Controls.FileUpload
{
    public class TempFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}