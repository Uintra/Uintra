using System;

namespace uCommunity.Navigation.DefaultImplementation
{
    public class MyLinkItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}
