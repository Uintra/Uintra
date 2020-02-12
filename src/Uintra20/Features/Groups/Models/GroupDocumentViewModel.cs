using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Member.Models;

namespace Uintra20.Features.Groups.Models
{
    public class GroupDocumentViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public MemberViewModel Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public bool CanDelete { get; set; }
        public string FileUrl { get; set; }
    }
}