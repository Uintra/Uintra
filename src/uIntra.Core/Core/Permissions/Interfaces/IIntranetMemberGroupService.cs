using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.Interfaces
{
    public interface IIntranetMemberGroupService
    {
        IntranetMemberGroup[] GetAll();
        IntranetMemberGroup[] GetForMember(int id);
    }
}