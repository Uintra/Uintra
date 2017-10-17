using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.FileUpload
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public IIntranetType Type { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
    }
}