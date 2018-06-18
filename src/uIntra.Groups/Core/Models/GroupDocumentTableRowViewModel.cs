using System;

namespace uIntra.Groups
{
    public class GroupDocumentTableRowViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool CanDelete { get; set; }
        public string FileUrl { get; set; }
    }
}