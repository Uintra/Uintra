using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class MemberNotifiersSettingsService : IMemberNotifiersSettingsService
    {
        private readonly ISqlRepository<MemberNotifierSetting> _repository;

        public MemberNotifiersSettingsService(ISqlRepository<MemberNotifierSetting> repository)
        {
            _repository = repository;
        }

        public IDictionary<NotifierTypeEnum, bool> GetForMember(Guid memberId)
        {
            var dbNotifiers = _repository.FindAll(r => r.MemberId == memberId).ToList();
            var createdNotifiers = CreateAbsentSettings(memberId, dbNotifiers.Select(e => e.NotifierType));
            return dbNotifiers.Concat(createdNotifiers).ToDictionary(e => e.NotifierType, e => e.IsEnabled);
        }

        public void SetForMember(Guid memberId, NotifierTypeEnum  notifierType, bool isEnabled)
        {
            var dbEntry = _repository.Find(r => r.MemberId == memberId && r.NotifierType == notifierType);
            dbEntry.IsEnabled = isEnabled;
            _repository.Update(dbEntry);
        }

        public IDictionary<Guid, IEnumerable<NotifierTypeEnum>> GetForMembers(IEnumerable<Guid> memberIds)
        {
            return _repository.FindAll(e => memberIds.Contains(e.MemberId))
                .GroupBy(e => e.MemberId)
                .ToDictionary(e => e.Key, e => e.Select(n => n.NotifierType));
        }

        private IEnumerable<MemberNotifierSetting> CreateAbsentSettings(Guid memberId, IEnumerable<NotifierTypeEnum> existingSettings)
        {
            var absentSettings = GetAllEnumCases<NotifierTypeEnum>().Except(existingSettings);
            var newEntities = absentSettings
                .Select(s => new MemberNotifierSetting() {Id = Guid.NewGuid(), MemberId = memberId, NotifierType = s, IsEnabled = true})
                .ToList();
            newEntities.ForEach(e => _repository.Add(e));
            return newEntities;
        }

        private IEnumerable<T> GetAllEnumCases<T>()
        {
           return (IEnumerable<T>) Enum.GetValues(typeof(T));
        }
    }
}
