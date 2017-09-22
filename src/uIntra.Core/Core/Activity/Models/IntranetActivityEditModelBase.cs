using System;
using System.ComponentModel.DataAnnotations;
using uIntra.Core.Links;
using uIntra.Core.ModelBinders;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.Core.Activity
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
        public Guid CreatorId { get; set; }

        public IIntranetUser Creator { get; set; }

        public IIntranetType ActivityType { get; set; }

        [PropertyBinder(typeof(LinksBinder))]
        public IActivityLinks Links { get; set; }
    }
}