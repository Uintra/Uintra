using System.ComponentModel.DataAnnotations;

namespace Uintra20.Features.Location.Models
{
    public class ActivityLocationEditModel
    {
        public string Address { get; set; }
        [MaxLength(200)]
        public string ShortAddress { get; set; }
    }
}