using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.Sql;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.Persistence;
using Uintra.Core.User;

using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class BasePermissionsService : IBasePermissionsService
    {
        protected virtual string BasePermissionCacheKey => "BasePermissionCache";

        private readonly ISqlRepository<PermissionEntity> _permissionsRepository;
        private readonly IPermissionActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IPermissionActivityTypeProvider _activityTypeProvider;
        private readonly ICacheService _cacheService;
        private readonly IPermissionSettingsSchema _permissionSettingsSchema;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public BasePermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IPermissionActionTypeProvider intranetActionProvider,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionActivityTypeProvider activityTypeProvider,
            ICacheService cacheService,
            IPermissionSettingsSchema permissionSettingsSchema,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _permissionsRepository = permissionsRepository;
            _intranetActionTypeProvider = intranetActionProvider;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _activityTypeProvider = activityTypeProvider;
            _cacheService = cacheService;
            _permissionSettingsSchema = permissionSettingsSchema;
            _intranetMemberService = intranetMemberService;
        }

        protected IEnumerable<BasePermissionModel> CurrentCache
        {
            get
            {
                return _cacheService.GetOrSet(
                    BasePermissionCacheKey,
                    () => _permissionsRepository.GetAll().Apply(MapAll));
            }
            set
            {
                _cacheService.Set(BasePermissionCacheKey, value);
            }
        }

        public IEnumerable<BasePermissionModel> GetAll()
        {
            return CurrentCache;
        }

        public IEnumerable<BasePermissionModel> GetForGroup(IntranetMemberGroup group)
        {
            var storedPerms = GetAll().Where(i => i.Group.Id == group.Id);

            var settings = _permissionSettingsSchema.Settings
                .Select(settingIdentity => Optional(storedPerms.FirstOrDefault(i =>
                    i.ActionType.Equals(settingIdentity.ActionType) &&
                    i.ActivityType == settingIdentity.ActivityType &&
                    i.Group.Id == group.Id))
                    .IfNone(() => _permissionSettingsSchema.GetDefault(settingIdentity, group)));

            return settings;
        }

        private IEnumerable<BasePermissionModel> MapAll(IEnumerable<PermissionEntity> entities) =>
            entities.Select(Map);

        public BasePermissionModel Save(BasePermissionUpdateModel update)
        {
            var intranetActionId = update.Action.ToInt();
            var activityTypeId = update.ActivityType.ToNullableInt();
            var storedEntity = _permissionsRepository
                //.FindOrNone(AndAlso(GroupIs(update.Group), ActionIs(update.Action)));
                .FindOrNone(i => i.IntranetMemberGroupId == update.Group.Id &&
                    i.IntranetActionId == intranetActionId &&
                    i.ActivityTypeId == activityTypeId);

            var actualEntity = storedEntity.Match(
                entity =>
                {
                    var updatedEntity = UpdateEntity(entity, update);
                    _permissionsRepository.Update(updatedEntity);
                    return updatedEntity;
                },
                () =>
                {
                    var createdEntity = CreateEntity(update);
                    _permissionsRepository.Add(createdEntity);
                    return createdEntity;
                });

            var actualMappedEntity = Map(actualEntity);
            CurrentCache = CurrentCache.WithUpdatedElement(e => e.Id, actualMappedEntity);

            return actualMappedEntity;
        }

        public void DeletePermissionsForMemberGroup(int memberGroupId)
        {
            _permissionsRepository.Delete(i => i.IntranetMemberGroupId == memberGroupId);
            CurrentCache = CurrentCache.Where(i => i.Group.Id != memberGroupId);
            //CurrentCache = null;
        }

        public bool Check(IIntranetMember member, PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction)
        {
            var permission = GetAll().Find(p =>
                p.Group.Id == member.Group.Id && p.ActionType.Equals(permissionAction) &&
                p.ActivityType.Some(i => i.Equals(permissionActivityType)).None(() => false)
            );

            //permission.IfNone(() =>
            //    throw new Exception($"action: [{permissionAction.ToString()}] for member group name: [{member.Group.Name}] under activity type: [{permissionActivityType.ToString()}] doesn't exist!"));

            var isAllowed = permission.Map(p => p.IsAllowed).Some(s => s).None(() => false);
            return isAllowed;
        }

        public bool Check(PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction)
        {
            var member = _intranetMemberService.GetCurrentMember();
            return Check(member, permissionActivityType, permissionAction);
        }

        protected BasePermissionModel Map(PermissionEntity entity) =>
            BasePermissionModel.Of(
                _intranetMemberGroupProvider.IntTypeDictionary,
                _intranetActionTypeProvider.IntTypeDictionary,
                _activityTypeProvider.IntTypeDictionary,
                entity);

        public static PermissionEntity UpdateEntity(PermissionEntity entity, BasePermissionUpdateModel update)
        {
            entity.IsAllowed = update.SettingValues.IsAllowed;
            entity.IsEnabled = update.SettingValues.IsEnabled;

            return entity;
        }

        public static PermissionEntity CreateEntity(BasePermissionUpdateModel update) =>
            new PermissionEntity
            {
                Id = Guid.NewGuid(),
                IntranetMemberGroupId = update.Group.Id,
                IntranetActionId = update.Action.ToInt(),
                ActivityTypeId = update.ActivityType.ToNullableInt(),
                IsAllowed = update.SettingValues.IsAllowed,
                IsEnabled = update.SettingValues.IsEnabled
            };

        //public static Expression<Func<PermissionEntity, bool>> GroupIs(IntranetMemberGroup group)
        //{
        //    var groupId = group.Id;
        //    return entity => entity.IntranetMemberGroupId == groupId;
        //}

        //public static Expression<Func<PermissionEntity, bool>> ActionIs(Enum action)
        //{
        //    var actionTypeId = action.ToInt();
        //    return entity => entity.IntranetMemberGroupId == actionTypeId;
        //}
    }
}