using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryItemViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string PreviewUrl { get; set; }
        public IIntranetType Type { get; set; }
        public string Extension { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsHidden { get; set; }
    }
}