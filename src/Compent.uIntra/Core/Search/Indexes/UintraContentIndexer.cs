using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Grid;
using Uintra.Search;
using Uintra.Tagging.UserTags;
using Uintra.Tagging.UserTags.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using static Compent.Uintra.Core.Search.Indexes.UintraGridEditorConstants;
using static Uintra.Core.Constants.GridEditorConstants;

namespace Compent.Uintra.Core.Search.Indexes
{
    public class UintraContentIndexer : IIndexer, IContentIndexer
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IElasticUintraContentIndex _contentIndex;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IUserTagProvider _tagProvider;
        private readonly IGridHelper _gridHelper;

        public UintraContentIndexer(
            UmbracoHelper umbracoHelper,
            ISearchUmbracoHelper searchUmbracoHelper,
            IElasticUintraContentIndex contentIndex,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGridHelper gridHelper,
            IUserTagProvider tagProvider)
        {
            _umbracoHelper = umbracoHelper;
            _searchUmbracoHelper = searchUmbracoHelper;
            _contentIndex = contentIndex;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _gridHelper = gridHelper;
            _tagProvider = tagProvider;
        }

        public void FillIndex()
        {
            var tags = _tagProvider.GetAll();

            var homePage = _umbracoHelper.TypedContentAtRoot().First(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));
            var contentPages = homePage.Descendants(_documentTypeAliasProvider.GetContentPage());

            var searchableContents = contentPages
                .Where(pc => _searchUmbracoHelper.IsSearchable(pc))
                .Select(pc => GetSearchableContent(pc, tags)).ToList();

            _contentIndex.Index(searchableContents);
        }

        public void FillIndex(int id)
        {
            var tags = _tagProvider.GetAll();

            var publishedContent = _umbracoHelper.TypedContent(id);
            if (publishedContent != null)
            {
                var isSearchable = _searchUmbracoHelper.IsSearchable(publishedContent);
                if (isSearchable)
                {
                    _contentIndex.Delete(publishedContent.Id);
                    _contentIndex.Index(GetSearchableContent(publishedContent, tags));
                }
                else
                {
                    _contentIndex.Delete(publishedContent.Id);
                }
            }
        }

        public void DeleteFromIndex(int id)
        {
            _contentIndex.Delete(id);
        }

        private SearchableUintraContent GetSearchableContent(IPublishedContent publishedContent, IEnumerable<UserTag> tags)
        {
            (List<string> content, List<string> titles, List<UserTagBackofficeViewModel> userTags) = ParseContentPanels(publishedContent);

            var userTagNames = userTags
                .Where(tag => tag.Selected)
                .Join(tags,
                    selectedTag => selectedTag.Id,
                    tag => tag.Id,
                    (selectedTag, tag) => tag.Text);

            return new SearchableUintraContent
            {
                Id = publishedContent.Id,
                Type = SearchableTypeEnum.Content.ToInt(),
                Url = publishedContent.Url,
                Title = publishedContent.Name,
                PanelContent = content,
                PanelTitle = titles,
                UserTagNames = userTagNames
            };
        }

        private (List<string> content, List<string> titles, List<UserTagBackofficeViewModel> userTags) ParseContentPanels(IPublishedContent publishedContent)
        {
            var titles = new List<string>();
            var content = new List<string>();
            var userTags = new List<UserTagBackofficeViewModel>();

            var values = _gridHelper.GetValues(
                publishedContent,
                ContentPanelAlias,
                GlobalPanelPickerAlias,
                UsersTagsAlias,
                ArticleStartAlias,
                ArticleContinueAlias,
                DocumentLibraryPanelAlias);

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

                    dynamic tags = panel.usersTags;
                    if (tags != null)
                        userTags.AddRange(tags.ToObject<IEnumerable<UserTagBackofficeViewModel>>());
                }
            }

            return (titles, content, userTags);
        }

        private dynamic GetContentPanelFromGlobal(dynamic value)
        {
            return _umbracoHelper.TypedContent((int) value.id)?.GetPropertyValue<dynamic>(PanelConfigPropertyAlias)?.value;
        }
    }

    public static class UintraGridEditorConstants
    {
        public const string UsersTagsAlias = "custom.UsersTags";
        public const string ArticleStartAlias = "custom.ArticleStart";
        public const string ArticleContinueAlias = "custom.ArticleContinue";
        public const string DocumentLibraryPanelAlias = "custom.DocumentLibraryPanel";
    }
}