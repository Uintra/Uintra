using System;

namespace uIntra.Search.Core.Entities
{
    public class SearchableActivity : SearchableBase
    {
        public string Teaser { get; set; }

        public string Description { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}