using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IBasePermissionsService
    {
        IReadOnlyCollection<BasePermissionModel> GetAll();
        BasePermissionModel Save(BasePermissionUpdateModel update);
    }
}
