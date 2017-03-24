using Umbraco.Core.Models;

namespace uCommunity.Users
{
    public class IntranetUser : IntranetUserBase
    {
        public string Email
        {
            get { return (string)this["Email"]; }
            set { this["Email"] = value; }
        }

        public string FirstName
        {
            get { return (string)this["FirstName"]; }
            set { this["FirstName"] = value; }
        }

        public string LastName
        {
            get { return (string)this["LastName"]; }
            set { this["LastName"] = value; }
        }

        public string Photo
        {
            get { return (string)this["Photo"]; }
            set { this["Photo"] = value; }
        }

        private readonly IntranetUserBase _wrapped;

        public IntranetUser()
        {
        }

        public IntranetUser(IntranetUserBase wrapped)
        {
            _wrapped = wrapped;
        }

        public override object this[string propertyName]
        {
            get
            {
                if (_wrapped == null)
                {
                    return base[propertyName];
                }
                return _wrapped[propertyName];
            }

            set
            {
                if (_wrapped == null)
                {
                    base[propertyName] = value;
                }
                else
                {
                    _wrapped[propertyName] = value;
                }
            }
        }
    }
}
