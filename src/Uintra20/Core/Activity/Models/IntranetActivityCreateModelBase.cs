using System;
using System.ComponentModel.DataAnnotations;
using Uintra20.Attributes;
using Uintra20.Features.Location.Models;

namespace Uintra20.Core.Activity.Models
{
    public class IntranetActivityCreateModelBase
    {
        [RequiredVirtual]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public ActivityLocationEditModel Location { get; set; }
    }
}