using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions
{
    public interface IIntranetMemberGroupService
    {
        IntranetMemberGroup[] GetAll();
        IntranetMemberGroup[] GetForMember(int id);
    }
}