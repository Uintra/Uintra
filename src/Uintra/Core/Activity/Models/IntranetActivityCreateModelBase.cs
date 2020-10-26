using System;
using System.ComponentModel.DataAnnotations;
using Uintra.Attributes;
using Uintra.Features.Location.Models;

namespace Uintra.Core.Activity.Models
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