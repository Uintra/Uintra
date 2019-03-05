using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.Permissions
{
    public enum PermissionActionEnum
    {
        View = 1,
        Create,
        Edit,
        Delete,
        Hide,
        [Display(Name = "Edit owner")]
        EditOwner,
        [Display(Name = "Edit other")]
        EditOther,
        [Display(Name = "Delete other")]
        DeleteOther,
        [Display(Name = "Hide other")]
        HideOther
    }
}
