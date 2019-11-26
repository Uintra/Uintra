using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Location.Models;

namespace Uintra20.Core.Activity.Models
{
    public class IntranetActivityEditModelBase
    {
        [Required]
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public IntranetActivityTypeEnum ActivityType { get; set; }

        public IActivityLinks Links { get; set; }

        public ActivityLocationEditModel Location { get; set; }
    }
}