using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace uIntra.Search
{
    public class SearchApplicationSettings : ConfigurationSection, ISearchApplicationSettings
    {
        private const string SearchIndexingDocumentTypesKey = "Search.IndexingDocumentTypes";

        public IEnumerable<string> IndexingDocumentTypesKey => ConfigurationManager.AppSettings[SearchIndexingDocumentTypesKey]
           .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
           .Select(el => el.Trim());
    }
}
