using uIntra.Core.Extentions;
using Umbraco.Core.Models;

namespace Compent.uIntra.Panels.DocumentLibraryPanel
{
    public class DocumentPanelMediaViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Extention { get; set; }        

        public string Url { get; set; }


        public static DocumentPanelMediaViewModel FromPublishedContent(IPublishedContent content)
        {
            return new DocumentPanelMediaViewModel
            {
                Id = content.Id,
                Title = content.Name,
                Extention = content.GetMediaExtention(),
                Url = content.Url
            };
        }
    }
}