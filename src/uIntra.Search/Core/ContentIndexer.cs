using System.Collections.Generic;
using System.Linq;
using uIntra.Search.Core.Entities;
using uIntra.Search.Core.Indexes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Search.Core
{ //TODO site implementation
    public class ContentIndexer : IIndexer, IContentIndexer
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IElasticContentIndex _contentIndex;

        public ContentIndexer(
            UmbracoHelper umbracoHelper,
            ISearchUmbracoHelper searchUmbracoHelper,
            IElasticContentIndex contentIndex)
        {
            _umbracoHelper = umbracoHelper;
            _searchUmbracoHelper = searchUmbracoHelper;
            _contentIndex = contentIndex;
        }

        public void FillIndex()
        {
            var rootPage = _umbracoHelper.TypedContentAtRoot().First();
            var publishedContents = _umbracoHelper.TypedContent(rootPage.Id).Descendants();
            var searchableContents = new List<SearchableContent>();

            foreach (var pc in publishedContents)
            {
                var isSearchable = _searchUmbracoHelper.IsSearchable(pc);
                if (isSearchable)
                {
                    searchableContents.Add(GetContent(pc));
                }
            }

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
            //var contentPage = (ContentPage)publishedContent;
            //dynamic grid = contentPage.Grid.ToString().Deserialize<dynamic>();

            //var titles = new List<string>();
            //var content = new List<string>();

            //foreach (var section in grid.sections)
            //{
            //    foreach (var row in section.rows)
            //    {
            //        foreach (var area in row.areas)
            //        {
            //            foreach (var control in area.controls)
            //            {
            //                if (control != null && control.editor != null && control.editor.view != null)
            //                {
            //                    if (control.editor.alias == "custom.ContentPanel")
            //                    {
            //                        string title = control.value.title;
            //                        if (!string.IsNullOrEmpty(title))
            //                        {
            //                            titles.Add(title.StripHtml());
            //                        }

            //                        string desc = control.value.description;
            //                        if (!string.IsNullOrEmpty(desc))
            //                        {
            //                            content.Add(desc.StripHtml());
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //return new SearchableContent
            //{
            //    Id = publishedContent.Id,
            //    Type = SearchableType.Content,
            //    Url = publishedContent.Url,
            //    Title = publishedContent.Name,
            //    PanelContent = content,
            //    PanelTitle = titles
            //};

            return null;
        }
    }
}
