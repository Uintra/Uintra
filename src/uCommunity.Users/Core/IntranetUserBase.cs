using System;
using System.Collections.Generic;
using uCommunity.Core.User;

namespace uCommunity.Users.Core
{
    public class IntranetUserBase: IIntranetUser
    {
        public Guid Id { get; set; }
        public int? UmbracoId { get; set; }
        public string DisplayedName { get; set; }

        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public virtual object this[string propertyName]
        {
            get { return _properties[propertyName]; }
            set { _properties[propertyName] = value; }
        }
    }
}
