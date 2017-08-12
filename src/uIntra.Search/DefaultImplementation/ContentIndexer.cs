using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Search
{
    public class ContentIndexer : IIndexer, IContentIndexer
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IElasticContentIndex _contentIndex;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public ContentIndexer(
            UmbracoHelper umbracoHelper,
            ISearchUmbracoHelper searchUmbracoHelper,
            IElasticContentIndex contentIndex,
            IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _searchUmbracoHelper = searchUmbracoHelper;
            _contentIndex = contentIndex;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public void FillIndex()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().First(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));
            var contentPages = homePage.Descendants(_documentTypeAliasProvider.GetContentPage());

            var searchableContents = contentPages
                .Where(pc => _searchUmbracoHelper.IsSearchable(pc))
                .Select(GetContent);

            _contentIndex.Index(searchableContents);
        }

        public void FillIndex(int id)
        {
            var publishedContent = _umbracoHelper.TypedContent(id);
            var isSearchable = _searchUmbracoHelper.IsSearchable(publishedContent);
            if (isSearchable)
            {
                _contentIndex.Delete(publishedContent.Id);
                _contentIndex.Index(GetContent(publishedContent));
            }
            else
            {
                _contentIndex.Delete(publishedContent.Id);
            }
        }

        public void DeleteFromIndex(int id)
        {
            _contentIndex.Delete(id);
        }

        private SearchableContent GetContent(IPublishedContent publishedContent)
        {
            dynamic grid = publishedContent.GetPropertyValue<JToken>("grid").ToString().Deserialize<dynamic>();

            var titles = new List<string>();
            var content = new List<string>();

            foreach (var section in grid.sections)
            {
                foreach (var row in section.rows)
                {
                    foreach (var area in row.areas)
                    {
                        foreach (var control in area.controls)
                        {
                            if (control != null && control.editor != null && control.editor.view != null)
                            {
                                if (control.editor.alias == "custom.ContentPanel")
                                {
                                    if (control.value != null)
                                    {
                                        string title = control.value.title;
                                        if (!string.IsNullOrEmpty(title))
                                        {
                                            titles.Add(title.StripHtml());
                                        }

                                        string desc = control.value.description;
                                        if (!string.IsNullOrEmpty(desc))
                                        {
                                            content.Add(desc.StripHtml());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new SearchableContent
            {
                Id = publishedContent.Id,
                Type = SearchableTypeEnum.Content.ToInt(),
                Url = publishedContent.Url,
                Title = publishedContent.Name,
                PanelContent = content,
                PanelTitle = titles
            };
        }
    }
}
