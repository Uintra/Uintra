using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public class MyLinksModel
    {
        public List<MyLinkItemModel> MyLinks { get; set; }

        public MyLinksModel()
        {
            MyLinks = new List<MyLinkItemModel>();
        }
    }
}
