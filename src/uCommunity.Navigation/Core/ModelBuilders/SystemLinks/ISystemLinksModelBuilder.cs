using System;
using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface ISystemLinksModelBuilder
    {
        IEnumerable<SystemLinksModel> Get(string contentXPath, string titleNodePropertyAlias,
            string linksNodePropertyAlias, string sortOrderNodePropertyAlias, Func<SystemLinksModel, int> sort);
    }
}