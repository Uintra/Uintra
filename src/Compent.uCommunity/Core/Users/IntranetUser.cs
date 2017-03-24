using System;
using uCommunity.Core.User;

namespace Compent.uCommunity.Core.Users
{
    public class IntranetUser : IIntranetUser, IHaveCreator
    {
        public Guid Id { get; set; }
        public int? UmbracoId { get; set; }
        public string DisplayedName { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public IIntranetUser Creator { get; set; }
    }
}