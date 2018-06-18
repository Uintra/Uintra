namespace Uintra.Panels.Core.Models
{
    public class ContentPanelModel
    {
        public bool IsImportant { get; set; }

        public string Description { get; set; }

        public dynamic TitleLink { get; set; }

        public string Title { get; set; }

        public int? Image { get; set; }        

        public dynamic VideoLink { get; set; }

        public string VideoType { get; set; }

        public string ImageVideoSize { get; set; }

        public string LinksListTitle { get; set; }

        public dynamic Links { get; set; }        

        public string Files { get; set; }
    }
}
