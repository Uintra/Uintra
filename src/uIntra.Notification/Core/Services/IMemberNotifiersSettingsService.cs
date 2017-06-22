using System;
using System.Collections.Generic;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface IMemberNotifiersSettingsService
    {
        IEnumerable<NotifierTypeEnum> GetForMember(Guid memberId);
        IDictionary<Guid, IEnumerable<NotifierTypeEnum>> GetForMembers(IEnumerable<Guid> memberIds);
        void SetForMember(Guid memberId, IEnumerable<NotifierTypeEnum> newNotifiersTypes);  
    }
}