using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksModelBuilder
    {
        IEnumerable<MyLinkItemModel> GetMenu();
    }
}