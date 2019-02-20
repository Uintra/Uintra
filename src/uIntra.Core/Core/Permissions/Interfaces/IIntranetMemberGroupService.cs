using System.Collections.Generic;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions
{
    public interface IIntranetMemberGroupService
    {
        IEnumerable<IntranetMemberGroup> GetAll();
        IntranetMemberGroup[] GetForMember(int id);
        int Create(string name);
        void Save(int id, string name);
        void Delete(int id);
    }
}