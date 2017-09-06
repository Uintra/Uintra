using System;

namespace uIntra.Groups
{
    public class GroupMemberViewModel
    {
        public Guid Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneCountryCode { get; set; }
        public bool IsGroupAdmin { get; set; }
        public bool CanUnsubscribe { get; set; }
    }
}