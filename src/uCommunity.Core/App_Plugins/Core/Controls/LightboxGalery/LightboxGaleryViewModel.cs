namespace uCommunity.Core.App_Plugins.Core.Controls.LightboxGalery
{
    public class LightboxGaleryViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PreviewUrl { get; set; }
        public MediaTypeEnum Type { get; set; }
        public string Extention { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}