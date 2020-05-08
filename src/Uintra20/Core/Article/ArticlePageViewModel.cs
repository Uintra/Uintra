using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Property;
using Uintra20.Features.Breadcrumbs.Models;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.Article
{
    public class ArticlePageViewModel : UBaseline.Shared.ArticlePage.ArticlePageViewModel
    {
        public PropertyViewModel<bool> ShowInSubMenu { get; set; }
        public Guid? GroupId { get; set; }
        public IEnumerable<BreadcrumbViewModel> Breadcrumbs { get; set; } = Enumerable.Empty<BreadcrumbViewModel>();
    }
}