using System.Collections.Generic;

namespace Uintra.Navigation.MyLinks
{
    public interface IMyLinksModelBuilder
    {
        IEnumerable<MyLinkItemModel> GetMenu();
    }
}