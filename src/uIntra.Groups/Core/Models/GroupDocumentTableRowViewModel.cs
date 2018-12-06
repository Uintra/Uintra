using System;
using Uintra.Core.User;

namespace Uintra.Groups
{
    public class GroupDocumentTableRowViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public IIntranetUser Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public bool CanDelete { get; set; }
        public string FileUrl { get; set; }
    }
}