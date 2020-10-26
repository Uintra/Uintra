using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Contracts;

namespace Uintra20.Core.Member.Profile
{
    public class ProfileModel : IContentWithMediaCreateEditModel
    {
        public Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        public int? PhotoId { get; set; }
        public string Email { get; set; }
        public string ProfileUrl { get; set; }
        public string NewMedia { get; set; }
        public IEnumerable<Guid> TagIdsData { get; set; } = Enumerable.Empty<Guid>();
    }
}