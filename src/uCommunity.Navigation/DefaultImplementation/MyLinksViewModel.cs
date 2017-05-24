using System;
using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Navigation.DefaultImplementation
{
    public class MyLinksViewModel
    {
        public Guid? Id { get; set; }
        public int ContentId { get; set; }
        public bool IsLinked { get; set; }
        public IEnumerable<MyLinkItemViewModel> Links { get; set; } = Enumerable.Empty<MyLinkItemViewModel>();
    }
}
