using System;

namespace uIntra.Navigation
{
    public class MyLinkItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ContentId { get; set; }
        public bool IsCurrentPage { get; set; }
    }
}
