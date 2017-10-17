using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class MemberNotifiersSettingsService : IMemberNotifiersSettingsService
    {
        private readonly ISqlRepository<MemberNotifierSetting> _memberNotifierSettingRepository;

        public MemberNotifiersSettingsService(ISqlRepository<MemberNotifierSetting> repository)
        {
            _memberNotifierSettingRepository = repository;
        }

        public IDictionary<NotifierTypeEnum, bool> GetForMember(Guid memberId)
        {
            var dbNotifiers = _memberNotifierSettingRepository.FindAll(r => r.MemberId == memberId).ToList();
            var createdNotifiers = CreateAbsentSettings(memberId, dbNotifiers.Select(e => e.NotifierType));
            return dbNotifiers.Concat(createdNotifiers).ToDictionary(e => e.NotifierType, e => e.IsEnabled);
        }

        public void SetForMember(Guid memberId, NotifierTypeEnum notifierType, bool isEnabled)
        {
            var dbEntry = _memberNotifierSettingRepository
                .FindAll(e => e.MemberId == memberId)
                .First(e => e.NotifierType == notifierType);
            dbEntry.IsEnabled = isEnabled;
            _memberNotifierSettingRepository.Update(dbEntry);
        }

        public IDictionary<Guid, IEnumerable<NotifierTypeEnum>> GetForMembers(IEnumerable<Guid> memberIds)
        {
            var memberIdsList = memberIds.ToList();
            memberIdsList.ForEach(SetupAbsentSettings);
            var memberNotifierSettings = _memberNotifierSettingRepository.FindAll(e => memberIdsList.Contains(e.MemberId));
            var result = memberNotifierSettings
                .GroupBy(e => e.MemberId)
                .ToDictionary(e => e.Key, e => e
                    .Where(n => n.IsEnabled)
                    .Select(n => n.NotifierType));
            return result;
        }


        private void SetupAbsentSettings(Guid memberId)
        {
            var dbNotifiers = _memberNotifierSettingRepository.FindAll(r => r.MemberId == memberId).ToList();
            CreateAbsentSettings(memberId, dbNotifiers.Select(e => e.NotifierType));
        }

        private IEnumerable<MemberNotifierSetting> CreateAbsentSettings(Guid memberId,
            IEnumerable<NotifierTypeEnum> existingSettings)
        {
            var absentSettings = EnumExtensions.GetEnumCases<NotifierTypeEnum>().Except(existingSettings);
            var newEntities = absentSettings
                .Select(s => new MemberNotifierSetting()
                {
                    Id = Guid.NewGuid(),
                    MemberId = memberId,
                    NotifierType = s,
                    IsEnabled = true
                })
                .ToList();
            _memberNotifierSettingRepository.Add(newEntities);
            return newEntities;
        }
    }
}
