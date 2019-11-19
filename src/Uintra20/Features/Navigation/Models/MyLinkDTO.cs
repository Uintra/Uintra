using System;

namespace Uintra20.Features.Navigation.Models
{
    public class MyLinkDTO
    {
        public Guid UserId { get; set; }

        public int ContentId { get; set; }

        public string QueryString { get; set; }

        public Guid? ActivityId { get; set; }
    }
}