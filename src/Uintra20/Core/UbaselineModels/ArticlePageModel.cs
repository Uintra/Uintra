﻿using UBaseline.Shared.Property;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Core.UbaselineModels
{
    public class ArticlePageModel:UBaseline.Shared.ArticlePage.ArticlePageModel, IUintraNavigationComposition
    {
        public PropertyModel<bool> ShowInSubMenu { get; set; }
    }
}