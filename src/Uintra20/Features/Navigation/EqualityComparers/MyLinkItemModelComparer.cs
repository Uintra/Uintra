using System.Collections.Generic;
using Uintra20.Features.Navigation.Models.MyLinks;

namespace Uintra20.Features.Navigation.EqualityComparers
{
    public class MyLinkItemModelComparer : IEqualityComparer<MyLinkItemModel>
    {
        public bool Equals(MyLinkItemModel x, MyLinkItemModel y) =>
            x != null &&
            y != null &&
            GetHashCode(x) == GetHashCode(y);

        public int GetHashCode(MyLinkItemModel obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}