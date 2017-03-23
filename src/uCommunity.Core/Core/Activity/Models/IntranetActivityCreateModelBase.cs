using System.ComponentModel.DataAnnotations;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivityCreateModelBase
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }
    }
}