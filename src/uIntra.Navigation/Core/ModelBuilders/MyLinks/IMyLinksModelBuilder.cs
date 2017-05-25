using System.Collections.Generic;

namespace uIntra.Navigation.MyLinks
{
    public interface IMyLinksModelBuilder
    {
        IEnumerable<MyLinkItemModel> GetMenu();
    }
}