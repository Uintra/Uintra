﻿using System;
using System.Collections.Generic;
using Uintra.Core.User.Models;
using Uintra.Features.Groups;
using Uintra.Features.Permissions.Models;

namespace Uintra.Core.Member.Entities
{
    public class IntranetMember : IGroupMember
    {
        public Guid Id { get; set; }
        public virtual string DisplayedName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        public int? PhotoId { get; set; }
        public IntranetMemberGroup[] Groups { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
        public IIntranetUser RelatedUser { get; set; }
        public int UmbracoId { get; set; }
        public bool Inactive { get; set; }
        public bool IsSuperUser { get; set; }
    }
}