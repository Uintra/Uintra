using System;

namespace uCommunity.Core.App_Plugins.Core.User
{
    public abstract class IntranetUserBase
    {
        public Guid Id { get; set; }
        public int? UmbracoId { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
    }
}