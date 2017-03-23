using System.ComponentModel.DataAnnotations;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivityCreateModelBase
    {
        [Required]
        public virtual string Title { get; set; }
    }
}