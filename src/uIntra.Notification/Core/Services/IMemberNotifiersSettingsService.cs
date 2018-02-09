using System;
using System.Collections.Generic;

namespace Uintra.Notification
{
    public interface IMemberNotifiersSettingsService
    {
        IDictionary<Enum, bool> GetForMember(Guid memberId);
        IDictionary<Guid, IEnumerable<Enum>> GetForMembers(IEnumerable<Guid> memberIds);
        void SetForMember(Guid memberId, Enum notifierType, bool isEnabled);
    }
}