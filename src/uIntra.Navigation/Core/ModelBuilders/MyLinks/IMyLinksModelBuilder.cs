using System.Collections.Generic;

namespace uIntra.Navigation.Core.MyLinks
{
    public interface IMyLinksModelBuilder
    {
        IEnumerable<MyLinkItemModel> GetMenu();
    }
}