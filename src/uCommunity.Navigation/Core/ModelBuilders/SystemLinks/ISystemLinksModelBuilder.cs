using System;

namespace uCommunity.Navigation.Core
{
    public interface ISystemLinksModelBuilder
    {
        SystemLinksModel Get(string systemLinksContentXPath, string pageTitleNodePropertyAlias, string pageUrlNodePropertyAlias, Func<SystemLinkItemModel, int> sort);
    }
}