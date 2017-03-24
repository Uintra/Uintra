using System;
using System.ComponentModel.DataAnnotations;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivitySaveModelBase
    {
        [Required]
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
    }
}