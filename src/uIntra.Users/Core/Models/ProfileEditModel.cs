using System;
using System.Collections.Generic;
using uIntra.Core.Media;

namespace uIntra.Users
{
    public class ProfileEditModel : IContentWithMediaCreateEditModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }

        public int? MediaRootId { get; set; }
        public string NewMedia { get; set; }
        public IDictionary<Enum, bool> MemberNotifierSettings { get; set; }
    }
}
