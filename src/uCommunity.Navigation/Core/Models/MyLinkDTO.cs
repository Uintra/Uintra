using System;

namespace uCommunity.Navigation.Core.Models
{
    public class MyLinkDTO
    {
        public Guid UserId { get; set; }

        public int ContentId { get; set; }

        public string QueryString { get; set; }
    }
}
