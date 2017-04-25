using System.Collections.Generic;

namespace uCommunity.Navigation.DefaultImplementation
{
    public class MyLinksViewModel
    {
        public List<MyLinkItemViewModel> MyLinks { get; set; }

        public string PageName { get; set; }

        public bool IsLinked { get; set; }

        public MyLinksViewModel()
        {
            MyLinks = new List<MyLinkItemViewModel>();
        }
    }
}
