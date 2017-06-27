using System;
using System.Collections.Generic;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface IMemberNotifiersSettingsService
    {
        IDictionary<NotifierTypeEnum, bool> GetForMember(Guid memberId);
        IDictionary<Guid, IEnumerable<NotifierTypeEnum>> GetForMembers(IEnumerable<Guid> memberIds);
        void SetForMember(Guid memberId, NotifierTypeEnum notifierType, bool isEnabled);
    }
}