using System;
using System.ComponentModel.DataAnnotations;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivityEditModelBase
    {
        [Required]
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public int PinDays { get; set; }

        public DateTime? EndPinDate { get; set; }
    }
}