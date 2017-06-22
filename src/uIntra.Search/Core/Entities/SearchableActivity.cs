using System;

namespace uIntra.Search.Core
{
    public class SearchableActivity : SearchableBase
    {
        public string Description { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}