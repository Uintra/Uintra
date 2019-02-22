using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra.Core.Permissions
{
    public enum PermissionActionEnum
    {
        View = 1,
        Create,
        Edit,
        Delete,
        [Display(Name = "Can edit owner")]
        CanEditOwner
    }
}
