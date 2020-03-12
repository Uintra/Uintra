using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Search.Indexes;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Grid;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Search
{
    public class ContentIndexer : IIndexer, IContentIndexer
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IElasticContentIndex _contentIndex;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGridHelper _gridHelper;

        public ContentIndexer(
            UmbracoHelper umbracoHelper,
            ISearchUmbracoHelper searchUmbracoHelper,
            IElasticContentIndex contentIndex,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGridHelper gridHelper)
        {
            _umbracoHelper = umbracoHelper;
            _searchUmbracoHelper = searchUmbracoHelper;
            _contentIndex = contentIndex;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _gridHelper = gridHelper;
        }

        public void FillIndex()
        {
            var homePage = _umbracoHelper.ContentAtRoot().First(pc => pc.ContentType.Alias.Equals(_documentTypeAliasProvider.GetHomePage()));
            var contentPages = homePage.DescendantsOfType(_documentTypeAliasProvider.GetArticlePage());

            var searchableContents = contentPages
                .Where(pc => _searchUmbracoHelper.IsSearchable(pc))
                .Select(GetContent);

            _contentIndex.Index(searchableContents);
        }

        public void FillIndex(int id)
        {
            var publishedContent = _umbracoHelper.Content(id);
            if (publishedContent == null) return;

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
            var (content, titles) = ParseContentPanels(publishedContent);


            return new SearchableContent
            {
                Id = publishedContent.Id,
                Type = SearchableTypeEnum.Content.ToInt(),
                Url = publishedContent.Url.ToLinkModel(),
                Title = publishedContent.Name,
                PanelContent = content,
                PanelTitle = titles
            };
        }

        private (List<string> content, List<string> titles) ParseContentPanels(IPublishedContent publishedContent)
        {
            var titles = new List<string>();
            var content = new List<string>();
            var values = _gridHelper.GetValues(publishedContent, GridEditorConstants.ContentPanelAlias, GridEditorConstants.GlobalPanelPickerAlias);

            foreach (var control in values)
            {
                if (control.value != null)
                {
                    dynamic panel = control.alias == GridEditorConstants.GlobalPanelPickerAlias
                        ? GetContentPanelFromGlobal(control.value)
                        : control.value;
                    if (panel == null) continue;

                    string title = panel.title;
                    if (!string.IsNullOrEmpty(title))
                        titles.Add(title.StripHtml());

                    string desc = panel.description;
                    if (!string.IsNullOrEmpty(desc))
                        content.Add(desc.StripHtml());
                }
            }

            return (titles, content);
        }


        private dynamic GetContentPanelFromGlobal(dynamic value) =>
            _umbracoHelper.Content((int)value.id)?
            .GetProperty(GridEditorConstants.PanelConfigPropertyAlias)?.Value<dynamic>()?
            .value;
    }
}
