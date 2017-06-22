using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class MemberNotifiersSettingsService : IMemberNotifiersSettingsService
    {
        private readonly ISqlRepository<MemberNotifierSettingDataModel> _repository;

        public MemberNotifiersSettingsService(ISqlRepository<MemberNotifierSettingDataModel> repository)
        {
            _repository = repository;
        }

        public IEnumerable<NotifierTypeEnum> GetForMember(Guid memberId)
        {
            var dbNotifiers = _repository.FindAll(r => r.MemberId == memberId).Select(r => r.NotifierType);
            return dbNotifiers;
        }

        public void SetForMember(Guid memberId, IEnumerable<NotifierTypeEnum> newNotifiersTypes)
        {
            var newNotifiersTypesList = newNotifiersTypes as IList<NotifierTypeEnum> ?? newNotifiersTypes.ToList();

            var dbEntries = _repository.FindAll(r => r.MemberId == memberId).ToList();
            var dbNotifiersTypes = dbEntries.Select(r => r.NotifierType).ToList();           
            var dbNotifiersTypesToAdd = newNotifiersTypesList.Except(dbNotifiersTypes).ToList();
            var dbNotifiersTypesToRemove = dbNotifiersTypes.Except(newNotifiersTypesList).ToList();

            dbNotifiersTypesToAdd
                .ToList()
                .ForEach(n => _repository.Add(new MemberNotifierSettingDataModel() {MemberId = memberId, NotifierType = n}));

            dbEntries
                .Where(e => dbNotifiersTypesToRemove.Contains(e.NotifierType))
                .ToList()
                .ForEach(e => _repository.DeleteById(e.Id));
        }

        public IDictionary<Guid, IEnumerable<NotifierTypeEnum>> GetForMembers(IEnumerable<Guid> memberIds)
        {
            return _repository.FindAll(e => memberIds.Contains(e.MemberId))
                .GroupBy(e => e.MemberId)
                .ToDictionary(e => e.Key, e => e.Select(n => n.NotifierType));
        }
    }
}
