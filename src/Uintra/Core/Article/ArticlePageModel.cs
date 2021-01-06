using UBaseline.Shared.Property;
using Uintra.Features.Navigation.Models;

namespace Uintra.Core.Article
{
    public class ArticlePageModel:UBaseline.Shared.ArticlePage.ArticlePageModel, IUintraNavigationComposition
    {
        public PropertyModel<bool> ShowInMenu { get; set; }
    }
}