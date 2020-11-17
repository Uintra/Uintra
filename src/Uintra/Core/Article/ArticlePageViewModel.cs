using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Property;
using Uintra.Features.Breadcrumbs.Models;
using Uintra.Features.Navigation.Models;

namespace Uintra.Core.Article
{
    public class ArticlePageViewModel : UBaseline.Shared.ArticlePage.ArticlePageViewModel
    {
        public PropertyViewModel<bool> ShowInSubMenu { get; set; }
        public Guid? GroupId { get; set; }
        public IEnumerable<BreadcrumbViewModel> Breadcrumbs { get; set; } = Enumerable.Empty<BreadcrumbViewModel>();
        public SubNavigationMenuItemModel SubNavigation { get; set; }

        public PropertyModel<bool> AllowAccess { get; set; }
    }
}