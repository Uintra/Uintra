using System.Collections.Generic;
using Uintra.Features.Breadcrumbs.Models;

namespace Uintra.Features.Breadcrumbs.Services.Contracts
{
    public interface IBreadcrumbService
    {
        IEnumerable<BreadcrumbViewModel> GetBreadcrumbs();
    }
}