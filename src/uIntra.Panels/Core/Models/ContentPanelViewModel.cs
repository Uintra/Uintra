using System.Collections.Generic;
using Uintra.BaseControls;

namespace Uintra.Panels.Core.Models
{
    public class ContentPanelViewModel
    {
        public bool IsImportant { get; set; }
        public string TitleLink { get; set; }
        public string Title { get; set; }
        public bool HasTitle { get; set; }
        public string Target { get; set; }

        public string Description { get; set; }

        public bool HasMedia { get; set; }
        public string PosterImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public bool ShowAsLightbox { get; set; }
        public string ImageVideoSize { get; set; }


        public VideoSourceTypes VideoSourceType { get; set; }
        public string VideoTooltip { get; set; }
        public string VideoSrc { get; set; }
        public string AutoplayVideo { get; set; }
        public string AutoplayIframe { get; set; }
        public string VideoLinkAlternativeText { get; set; }
        public string EmbedUrl { get; set; }

        public string LinksListTitle { get; set; }
        public List<ContentPanelLinkViewModel> Links { get; set; } = new List<ContentPanelLinkViewModel>();
        public List<ContentPanelFileViewModel> Files { get; set; } = new List<ContentPanelFileViewModel>();
    }
}
