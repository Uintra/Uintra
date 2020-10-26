using System;
using Uintra.Core.Member.Models;

namespace Uintra.Features.Groups.Models
{
    public class GroupDocumentViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public MemberViewModel Creator { get; set; }
        public string CreateDate { get; set; }
        public bool CanDelete { get; set; }
        public string FileUrl { get; set; }
    }
}