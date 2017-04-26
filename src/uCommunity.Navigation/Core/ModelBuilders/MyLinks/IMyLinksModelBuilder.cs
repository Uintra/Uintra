using System;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksModelBuilder
    {
        MyLinksModel Get(Func<MyLinkItemModel, string> sort);
    }
}