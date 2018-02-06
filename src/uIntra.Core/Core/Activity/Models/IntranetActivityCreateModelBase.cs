using System;
using System.ComponentModel.DataAnnotations;
using uIntra.Core.Attributes;
using uIntra.Core.Links;
using uIntra.Core.Location;
using uIntra.Core.ModelBinders;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public class IntranetActivityCreateModelBase
    {
        [RequiredVirtual]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Enum ActivityType { get; set; }

        [PropertyBinder(typeof(LinksBinder))]
        public IActivityCreateLinks Links { get; set; }

        public ActivityLocationEditModel Location { get; set; }
    }
}