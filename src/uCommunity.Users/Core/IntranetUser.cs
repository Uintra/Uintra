using System;
using uCommunity.Core.User;

namespace uCommunity.Users.Core
{
    public class IntranetUser : IIntranetUser
    {
        public Guid Id { get; set; }
        public int? UmbracoId { get; set; }
        public virtual string DisplayedName
        {
            get { return $"{FirstName} {LastName}"; }
            set { throw new Exception("Can't set displayed name. You should change first and last name instead."); }
        }
        public virtual string Photo { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
