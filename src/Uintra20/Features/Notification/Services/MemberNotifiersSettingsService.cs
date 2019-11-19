using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Sql;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Notification.Services
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

        #region async

        public async Task<IDictionary<Enum, bool>> GetForMemberAsync(Guid memberId)
        {
            var dbNotifiers = (await _memberNotifierSettingRepository.FindAllAsync(r => r.MemberId == memberId)).ToList();
            var createdNotifiers = CreateAbsentSettings(
                memberId,
                dbNotifiers.Select(e => _notifierTypeProvider[e.NotifierType]),
                _notifierTypeProvider.All);

            return dbNotifiers.Concat(createdNotifiers).ToDictionary(e => _notifierTypeProvider[e.NotifierType], e => e.IsEnabled);
        }

        public async Task<IDictionary<Guid, IEnumerable<Enum>>> GetForMembersAsync(IEnumerable<Guid> memberIds)
        {
            var memberIdsList = memberIds.ToList();
            memberIdsList.ForEach(SetupAbsentSettings);
            var memberNotifierSettings = await _memberNotifierSettingRepository.FindAllAsync(e => memberIdsList.Contains(e.MemberId));
            var result = memberNotifierSettings
                .GroupBy(e => e.MemberId)
                .ToDictionary(e => e.Key, e => e
                    .Where(n => n.IsEnabled)
                    .Select(n => _notifierTypeProvider[n.NotifierType]));
            return result;
        }

        public async Task SetForMemberAsync(Guid memberId, Enum notifierType, bool isEnabled)
        {
            var dbEntry = await _memberNotifierSettingRepository
                .FindAsync(e => e.MemberId == memberId && e.NotifierType == notifierType.ToInt());
            dbEntry.IsEnabled = isEnabled;
            await _memberNotifierSettingRepository.UpdateAsync(dbEntry);
        }

        #endregion

        #region sync

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
            var absentSettings = allSettings.Except(existingSettings).ToList();
            var newEntities = absentSettings
                .Select(s => new MemberNotifierSetting()
                {
                    Id = Guid.NewGuid(),
                    MemberId = memberId,
                    NotifierType = s.ToInt(),
                    IsEnabled = s.ToInt() != NotifierTypeEnum.EmailNotifier.ToInt()
                })
                .ToList();
            _memberNotifierSettingRepository.Add(newEntities);
            return newEntities;
        }

        #endregion

    }
}