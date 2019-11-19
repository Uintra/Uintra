using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Location.Models;

namespace Uintra20.Features.Activity.Models
{
    public class IntranetActivityCreateModelBase
    {
        [Required]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Enum ActivityType { get; set; }

        public IActivityCreateLinks Links { get; set; }

        public ActivityLocationEditModel Location { get; set; }
    }
}