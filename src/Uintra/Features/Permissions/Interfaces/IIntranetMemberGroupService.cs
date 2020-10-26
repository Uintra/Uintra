using System.Collections.Generic;
using Uintra.Features.Permissions.Models;

namespace Uintra.Features.Permissions.Interfaces
{
    public interface IIntranetMemberGroupService
    {
        IEnumerable<IntranetMemberGroup> GetAll();
        IEnumerable<IntranetMemberGroup> GetForMember(int id);
        int Create(string name);
        bool Save(int id, string name);
        void Delete(int id);
        void RemoveFromAll(int memberId);
        void AssignDefaultMemberGroup(int memberId);
        void ClearCache();
    }
}
