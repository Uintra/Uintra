using System.Linq;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.User
{
    public class IntranetUserContentProvider : IIntranetUserContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly string _baseAlias;

        public IntranetUserContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _baseAlias = _documentTypeAliasProvider.GetHomePage();
        }

        public IPublishedContent GetProfilePage()
        {
			var baseContent = Umbraco.Web.Composing.Current.UmbracoHelper.ContentAtRoot().First(x => x.ContentType.Alias == _baseAlias);
			var profilePageAlias = _documentTypeAliasProvider.GetProfilePage();
            var profilePageContent = baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == profilePageAlias);

            return profilePageContent;
        }

        public IPublishedContent GetEditPage()
        {
	        var baseContent = Umbraco.Web.Composing.Current.UmbracoHelper.ContentAtRoot().First(x => x.ContentType.Alias == _baseAlias);
			var editPageAlias = _documentTypeAliasProvider.GetProfileEditPage();
            var editPageContent = baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == editPageAlias);
            
            return editPageContent;
        }
    }
}