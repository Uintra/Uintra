using System;
using System.Collections.Generic;
using Uintra20.Features.Media;

namespace Uintra20.Core.Member.Models
{
    public class ProfileEditModel : IContentWithMediaCreateEditModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        public int? PhotoId { get; set; }
        public string Email { get; set; }

        public string ProfileUrl { get; set; }
        public int? MediaRootId { get; set; }
        public string NewMedia { get; set; }
        public IDictionary<Enum, bool> MemberNotifierSettings { get; set; }
    }
}