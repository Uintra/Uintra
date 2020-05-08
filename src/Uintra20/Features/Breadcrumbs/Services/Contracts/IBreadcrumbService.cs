using System.Collections.Generic;
using Uintra20.Features.Breadcrumbs.Models;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Breadcrumbs.Services.Contracts
{
    public interface IBreadcrumbService
    {
        IEnumerable<BreadcrumbItemViewModel> GetBreadcrumbsItems();
    }
}