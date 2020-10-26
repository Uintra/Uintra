using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.Member.Profile.Edit.Models
{
    public class ProfileEditModel: ProfileModel
    {
        [Required(AllowEmptyStrings = false)]
        public override string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public override string LastName { get; set; }
    }
}