using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IActivityTypePermissionsService
    {
        IEnumerable<ActivityTypePermissionModel> GetAll();
        Unit Save(ActivityTypePermissionCreateModel createInfo);
    }
}