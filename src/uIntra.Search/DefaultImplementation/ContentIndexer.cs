using System.Collections.Generic;
using System.Linq;
using uIntra.Core;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.PagePromotion;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Web;
using Umbraco.Core.Services;
using static uIntra.Core.Constants.GridEditorConstants;

namespace uIntra.Search
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
            (List<string> content, List<string> titles) = ParseContentPanels(publishedContent);
            var pagePromotionContent = GetPagePromotionContent(publishedContent);

            content.AddRange(pagePromotionContent);

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

        private (List<string> content, List<string> titles) ParseContentPanels(IPublishedContent publishedContent)
        {
            var titles = new List<string>();
            var content = new List<string>();
            var values = _gridHelper.GetValues(publishedContent, ContentPanelAlias, GlobalPanelPickerAlias);

            foreach (var control in values)
            {
                if (control.value != null)
                {
                    dynamic panel = control.alias == GlobalPanelPickerAlias
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

        private List<string> GetPagePromotionContent(IPublishedContent publishedContent)
        {
            var content = new List<string>();

            var config = PagePromotionHelper.GetConfig(publishedContent);
            if (config == null) return content;

            content.Add(config.Title);
            content.Add(config.Description);

            return content;
        }



        public void ProcessContentPublished(IContentService sender, MoveEventArgs<IContent> args)
        {
            foreach (var arg in args.MoveInfoCollection) FillIndex(arg.Entity.Id);
        }

        private dynamic GetContentPanelFromGlobal(dynamic value) =>
            _umbracoHelper.TypedContent((int)value.id)?
            .GetPropertyValue<dynamic>(PanelConfigPropertyAlias)?
            .value;
    }
}
