using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.Notification
{
    public interface IMemberNotifiersSettingsService
    {
        IDictionary<Enum, bool> GetForMember(Guid memberId);
        IDictionary<Guid, IEnumerable<Enum>> GetForMembers(IEnumerable<Guid> memberIds);
        void SetForMember(Guid memberId, Enum notifierType, bool isEnabled);


        Task<IDictionary<Enum, bool>> GetForMemberAsync(Guid memberId);
        Task<IDictionary<Guid, IEnumerable<Enum>>> GetForMembersAsync(IEnumerable<Guid> memberIds);
        Task SetForMemberAsync(Guid memberId, Enum notifierType, bool isEnabled);
    }
}
