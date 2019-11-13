using System;

namespace Uintra20.Core.Navigation
{
    public class MyLinkDTO
    {
        public Guid UserId { get; set; }

        public int ContentId { get; set; }

        public string QueryString { get; set; }

        public Guid? ActivityId { get; set; }
    }
}