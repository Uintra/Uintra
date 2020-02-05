using System;

namespace Uintra20.Core.Controls.LightboxGallery
{
    public class LightboxGalleryItemPreviewModel
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string PreviewUrl { get; set; }
        public bool IsHidden { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Extension { get; set; }
        public Enum Type { get; set; }
    }
}