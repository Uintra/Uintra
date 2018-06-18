using System;

namespace uIntra.Core.User
{
    public class IntranetUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? NewMedia { get; set; }
        public bool DeleteMedia { get; set; }
    }
}