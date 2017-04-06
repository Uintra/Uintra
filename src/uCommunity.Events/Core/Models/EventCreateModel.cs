using System;
using System.ComponentModel.DataAnnotations;

namespace uCommunity.Events
{
    public class EventCreateModel : EventCreateModelBase
    {
        [Required]
        public Guid CreatorId { get; set; }
    }
}