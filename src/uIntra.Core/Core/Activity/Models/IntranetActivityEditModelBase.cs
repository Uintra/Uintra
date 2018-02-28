using System;
using System.ComponentModel.DataAnnotations;
using Uintra.Core.Attributes;
using Uintra.Core.Links;
using Uintra.Core.Location;
using Uintra.Core.ModelBinders;

namespace Uintra.Core.Activity
{
    public class IntranetActivityEditModelBase
    {
        [Required]
        public virtual Guid Id { get; set; }
        [RequiredVirtual]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }
        
        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Enum ActivityType { get; set; }

        [PropertyBinder(typeof(LinksBinder))]
        public IActivityLinks Links { get; set; }

        public ActivityLocationEditModel Location { get; set; }
    }
}