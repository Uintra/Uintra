using UBaseline.Shared.Property;
using Uintra.Core.Authentication;
using Uintra.Core.Authentication.Models;
using Uintra.Features.Navigation.Models;

namespace Uintra.Core.Article
{
    public class ArticlePageModel:UBaseline.Shared.ArticlePage.ArticlePageModel, IUintraNavigationComposition, IAnonymousAccessComposition
    {
        public PropertyModel<bool> ShowInSubMenu { get; set; }
        public PropertyModel<bool> AllowAccess { get; set; }
    }
}