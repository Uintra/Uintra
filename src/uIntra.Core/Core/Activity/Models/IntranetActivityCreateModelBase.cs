using System;
using System.ComponentModel.DataAnnotations;

namespace uIntra.Core.Activity
{
    public class IntranetActivityCreateModelBase
    {
        [Required]
        public virtual string Title { get; set; }

        public bool IsPinned { get; set; }

        public virtual DateTime? EndPinDate { get; set; }
    }
}