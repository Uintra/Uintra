using System;
using System.Collections.Specialized;

namespace uCommunity.Navigation.Core.Models
{
    public class MyLinkDTO
    {
        public Guid UserId { get; set; }

        public int ContentId { get; set; }

        public NameValueCollection QueryString { get; set; }
    }
}
