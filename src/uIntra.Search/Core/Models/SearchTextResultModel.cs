using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Search.Core.Models
{
    public class SearchTextResultModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> PanelContent { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public SearchTextResultModel()
        {        
            PanelContent = Enumerable.Empty<string>();
        }
    }
}