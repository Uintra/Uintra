﻿using Compent.Extensions;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.Navigation;
using UBaseline.Shared.Node;
using Uintra20.Features.Breadcrumbs.Models;
using Uintra20.Features.Breadcrumbs.Services.Contracts;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Breadcrumbs.Services.Implementations
{
    public class BreadcrumbService : IBreadcrumbService
    {
        private readonly IUBaselineRequestContext _uBaselineRequestContext;
        private readonly INodeModelService _nodeModelService;

        public BreadcrumbService(
            IUBaselineRequestContext uBaselineRequestContext,
            INodeModelService nodeModelService)
        {
            _uBaselineRequestContext = uBaselineRequestContext;
            _nodeModelService = nodeModelService;
        }

        public virtual IEnumerable<BreadcrumbItemViewModel> GetBreadcrumbsItems()
        {
            var pathToRoot = PathToRoot(_uBaselineRequestContext.Node).Reverse();

            var result = pathToRoot.Select(page =>
            {
                var navigationName = string.Empty;
                if (page is INavigationComposition composition)
                {
                    navigationName = composition.Navigation.NavigationTitle;
                }

                return new BreadcrumbItemViewModel
                {
                    Name = navigationName.HasValue()
                        ? navigationName
                        : page.Name,
                    Url = page.Url,
                    IsClickable = _uBaselineRequestContext.Node.Url != page.Url && !(page is HeadingPageModel)
                };
            });

            return result;
        }

        protected virtual IEnumerable<INodeModel> PathToRoot(INodeModel node)
        {
            var current = node;

            while (current != null)
            {
                yield return current;

                current = _nodeModelService.Get(current.ParentId);
            }
        }
    }
}