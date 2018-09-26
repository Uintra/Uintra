using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.Providers
{
    public class ContentPageContentProvider : ContentProviderBase, IContentPageContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IEnumerable<string> _baseXPath;

        public ContentPageContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper) : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _umbracoHelper = umbracoHelper;
            _baseXPath = new[] { _documentTypeAliasProvider.GetHomePage() };
        }

        public IEnumerable<IPublishedContent> GetAllContentPages()
        {
            var contentPages = GetDescendants(_baseXPath.Append(_documentTypeAliasProvider.GetContentPage())).ToList();
            return contentPages;
        }

        public IPublishedContent GetFirstUserListContentPage()
        {
            return GetAllContentPages().FirstOrDefault(i =>
            {
                var obj = i.GetPropertyValue<JObject>("grid");
                var tokens = obj.SelectTokens("sections[0].rows[0].areas[0].controls[*].editor.alias");
                return tokens.Any(j => 
                    j is JValue temp && temp != null && temp.Value?.ToString() == "custom.UserList");
            });
        }

        public IPublishedContent GetUserListContentPageFromPicker()
        {
            var homePage = GetContent(_baseXPath);
            return homePage.GetPropertyValue<IEnumerable<IPublishedContent>>("userListPage")?.FirstOrDefault();
        }
    }
}
