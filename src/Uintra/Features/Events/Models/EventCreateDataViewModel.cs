﻿using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Models;
using Uintra.Features.Links.Models;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Events.Models
{
    public class EventCreateDataViewModel
    {
        public MemberViewModel Creator { get; set; }
        public IEnumerable<IntranetMember> Members { get; set; } = Enumerable.Empty<IntranetMember>();
        public IEnumerable<UserTag> Tags { get; set; }

        public IActivityCreateLinks Links { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? GroupId { get; set; }

        public bool PinAllowed { get; set; }
        public bool CanEditOwner { get; set; }
    }
}