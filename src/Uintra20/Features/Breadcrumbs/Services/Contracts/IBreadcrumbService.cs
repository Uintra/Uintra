using System.Collections.Generic;
using Uintra20.Features.Breadcrumbs.Models;

namespace Uintra20.Features.Breadcrumbs.Services.Contracts
{
    public interface IBreadcrumbService
    {
        IEnumerable<BreadcrumbViewModel> GetBreadcrumbs();
    }
}