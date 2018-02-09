using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;

namespace Uintra.Notification
{
    public class MemberNotifiersSettingsService : IMemberNotifiersSettingsService
    {
        private readonly ISqlRepository<MemberNotifierSetting> _memberNotifierSettingRepository;
        private readonly INotifierTypeProvider _notifierTypeProvider;

        public MemberNotifiersSettingsService(ISqlRepository<MemberNotifierSetting> repository, INotifierTypeProvider notifierTypeProvider)
        {
            _memberNotifierSettingRepository = repository;
            _notifierTypeProvider = notifierTypeProvider;
        }

        public IDictionary<Enum, bool> GetForMember(Guid memberId)
        {
            var dbNotifiers = _memberNotifierSettingRepository.FindAll(r => r.MemberId == memberId).ToList();
            var createdNotifiers = CreateAbsentSettings(
                memberId,
                dbNotifiers.Select(e => _notifierTypeProvider[e.NotifierType]),
                _notifierTypeProvider.All);

            return dbNotifiers.Concat(createdNotifiers).ToDictionary(e => _notifierTypeProvider[e.NotifierType], e => e.IsEnabled);
        }

        public void SetForMember(Guid memberId, Enum notifierType, bool isEnabled)
        {
            var dbEntry = _memberNotifierSettingRepository
                .FindAll(e => e.MemberId == memberId)
                .First(e => e.NotifierType == notifierType.ToInt());
            dbEntry.IsEnabled = isEnabled;
            _memberNotifierSettingRepository.Update(dbEntry);
        }

        public IDictionary<Guid, IEnumerable<Enum>> GetForMembers(IEnumerable<Guid> memberIds)
        {
            var memberIdsList = memberIds.ToList();
            memberIdsList.ForEach(SetupAbsentSettings);
            var memberNotifierSettings = _memberNotifierSettingRepository.FindAll(e => memberIdsList.Contains(e.MemberId));
            var result = memberNotifierSettings
                .GroupBy(e => e.MemberId)
                .ToDictionary(e => e.Key, e => e
                    .Where(n => n.IsEnabled)
                    .Select(n => _notifierTypeProvider[n.NotifierType]));
            return result;
        }


        private void SetupAbsentSettings(Guid memberId)
        {
            var dbNotifiers = _memberNotifierSettingRepository.FindAll(r => r.MemberId == memberId).ToList();
            CreateAbsentSettings(memberId, dbNotifiers.Select(e => _notifierTypeProvider[e.NotifierType]), _notifierTypeProvider.All);
        }

        private IEnumerable<MemberNotifierSetting> CreateAbsentSettings(
            Guid memberId,
            IEnumerable<Enum> existingSettings,
            IEnumerable<Enum> allSettings)
        {
            var absentSettings = allSettings.Except(existingSettings);
            var newEntities = absentSettings
                .Select(s => new MemberNotifierSetting()
                {
                    Id = Guid.NewGuid(),
                    MemberId = memberId,
                    NotifierType = s.ToInt(),
                    IsEnabled = true
                })
                .ToList();
            _memberNotifierSettingRepository.Add(newEntities);
            return newEntities;
        }
    }
}
