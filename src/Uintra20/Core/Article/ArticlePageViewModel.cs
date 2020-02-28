using System;
using UBaseline.Shared.Property;

namespace Uintra20.Core.Article
{
    public class ArticlePageViewModel : UBaseline.Shared.ArticlePage.ArticlePageViewModel
    {
        public PropertyViewModel<bool> ShowInSubMenu { get; set; }
        public Guid? GroupId { get; set; }
    }
}