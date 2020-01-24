using System;
using System.Collections.Generic;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation.ModelBuilders.SystemLinks
{
    public interface ISystemLinksModelBuilder
    {
        IEnumerable<SharedLinkItemViewModel> Get();
    }
}