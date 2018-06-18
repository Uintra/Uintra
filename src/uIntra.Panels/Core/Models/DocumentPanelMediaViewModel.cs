using uIntra.Core.Extensions;
using Umbraco.Core.Models;

namespace uIntra.Panels.Core.Models
{
    public class DocumentPanelMediaViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Extension { get; set; }        

        public string Url { get; set; }

        public static DocumentPanelMediaViewModel FromPublishedContent(IPublishedContent content)
        {
            return new DocumentPanelMediaViewModel
            {
                Id = content.Id,
                Title = content.Name,
                Extension = content.GetMediaExtension(),
                Url = content.Url
            };
        }
    }
}