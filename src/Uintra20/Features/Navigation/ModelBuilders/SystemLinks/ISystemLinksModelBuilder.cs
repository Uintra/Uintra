using System;
using System.Collections.Generic;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation.ModelBuilders.SystemLinks
{
    public interface ISystemLinksModelBuilder
    {
        IEnumerable<SystemLinksModel> Get(IEnumerable<string> contentAliasPath, string titleNodePropertyAlias,
            string linksNodePropertyAlias, string sortOrderNodePropertyAlias, Func<SystemLinksModel, int> sort);
    }
}