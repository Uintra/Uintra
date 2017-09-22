using System;
using System.ComponentModel.DataAnnotations;
using uIntra.Core.Links;
using uIntra.Core.ModelBinders;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class IntranetActivityCreateModelBase
    {
        [RequiredVirtual]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }

        [Required]
        public Guid CreatorId { get; set; }

        public IIntranetUser Creator { get; set; }

        public IIntranetType ActivityType { get; set; }

        [PropertyBinder(typeof(LinksBinder))]
        public IActivityCreateLinks Links { get; set; }
    }
}

public class RequiredVirtualAttribute : RequiredAttribute
{
    public bool IsRequired { get; set; } = true;

    public override bool IsValid(object value)
    {
        if (IsRequired)
            return base.IsValid(value);
        return true;
    }

    public override bool RequiresValidationContext => IsRequired;
}