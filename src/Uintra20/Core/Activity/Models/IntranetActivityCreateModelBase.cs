using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Core.Links;
using Uintra20.Core.Location;

namespace Uintra20.Core.Activity
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