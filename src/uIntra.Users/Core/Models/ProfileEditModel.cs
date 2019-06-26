using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Uintra.Core.Media;

namespace Uintra.Users
{
    public class ProfileEditModel : IContentWithMediaCreateEditModel
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
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
