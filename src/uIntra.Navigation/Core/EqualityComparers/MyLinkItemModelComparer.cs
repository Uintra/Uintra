using System.Collections.Generic;

namespace Uintra.Navigation.EqualityComparers
{
    public class MyLinkItemModelComparer : IEqualityComparer<MyLinkItemModel>
    {
        public bool Equals(MyLinkItemModel x, MyLinkItemModel y)
        {
            return x != null
                    && y != null
                    && this.GetHashCode(x) == this.GetHashCode(y);
        }

        public int GetHashCode(MyLinkItemModel obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}
