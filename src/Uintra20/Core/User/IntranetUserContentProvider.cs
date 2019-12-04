using System.Linq;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.User
{
    public class IntranetUserContentProvider : IIntranetUserContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IPublishedContent _baseContent;

        public IntranetUserContentProvider(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            var baseAlias = _documentTypeAliasProvider.GetHomePage();
            _baseContent = umbracoHelper.ContentAtRoot().First(x => x.ContentType.Alias == baseAlias);
        }

        public IPublishedContent GetProfilePage()
        {
            var profilePageAlias = _documentTypeAliasProvider.GetProfilePage();
            var profilePageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == profilePageAlias);

            return profilePageContent;
        }

        public IPublishedContent GetEditPage()
        {
            var editPageAlias = _documentTypeAliasProvider.GetProfileEditPage();
            var editPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == editPageAlias);
            
            return editPageContent;
        }
    }
}