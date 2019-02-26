using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.Sql;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.Persistence;
using Uintra.Core.User;
using static Uintra.Core.Extensions.EnumerableExtensions;

namespace Uintra.Core.Permissions.Implementation
{
    public class PermissionsService : IPermissionsService
    {
        protected virtual string BasePermissionCacheKey => "PermissionCache";

        private readonly ISqlRepository<PermissionEntity> _permissionsRepository;
        private readonly IPermissionActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IPermissionActivityTypeProvider _activityTypeProvider;
        private readonly ICacheService _cacheService;
        private readonly IPermissionSettingsSchema _permissionSettingsSchema;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public PermissionsService(
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

        protected virtual IEnumerable<PermissionModel> CurrentCache
        {
            get => _cacheService.GetOrSet(
                    BasePermissionCacheKey,
                    () => _permissionsRepository.GetAll().Apply(MapAll));

            set => _cacheService.Set(BasePermissionCacheKey, value);
        }

        public virtual IEnumerable<PermissionModel> GetAll()
        {
            return CurrentCache;
        }

        public virtual IEnumerable<PermissionModel> GetForGroup(IntranetMemberGroup group)
        {
            var storedPerms = GetAll()
                .Where(i => i.Group.Id == group.Id)
                .ToDictionary(settingIdentity => (
                    settingIdentity.ActionType,
                    settingIdentity.ActivityType,
                    settingIdentity.Group.Id));

            var settings = _permissionSettingsSchema.Settings
                .Select(settingIdentity => storedPerms
                    .ItemOrNone((settingIdentity.ActionType, settingIdentity.ActivityType, group.Id))
                    .IfNone(() => _permissionSettingsSchema.GetDefault(settingIdentity, group)));

            return settings;
        }

        protected virtual IEnumerable<PermissionModel> MapAll(IEnumerable<PermissionEntity> entities) =>
            entities.Select(Map);

        public virtual PermissionModel Save(PermissionUpdateModel update)
        {
            var storedEntity = _permissionsRepository
                .FindOrNone(AndAlso(GroupIs(update.Group),
                    ActionIs(update.Action), 
                    ActivityTypeIs(update.ActivityType)));

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

        public virtual void DeletePermissionsForMemberGroup(int memberGroupId)
        {
            _permissionsRepository.Delete(i => i.IntranetMemberGroupId == memberGroupId);
            CurrentCache = CurrentCache.Where(i => i.Group.Id != memberGroupId);
            //CurrentCache = null;
        }

        public virtual bool Check(IIntranetMember member, PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction)
        {
            if (member.Group == null) return false;

            var permission = GetAll().Find(p =>
                p.Group.Id == member.Group.Id && p.ActionType.Equals(permissionAction) &&
                p.ActivityType == permissionActivityType);

            //permission.IfNone(() =>
            //    throw new Exception($"action: [{permissionAction.ToString()}] for member group name: [{member.Group.Name}] under activity type: [{permissionActivityType.ToString()}] doesn't exist!"));

            var isAllowed = permission.Match(p => p.IsAllowed, () => false);
            return isAllowed;
        }

        public virtual bool Check(PermissionActivityTypeEnum permissionActivityType, PermissionActionEnum permissionAction)
        {
            var member = _intranetMemberService.GetCurrentMember();
            return Check(member, permissionActivityType, permissionAction);
        }

        protected virtual PermissionModel Map(PermissionEntity entity) =>
            PermissionModel.Of(
                _intranetMemberGroupProvider.IntTypeDictionary,
                _intranetActionTypeProvider.IntTypeDictionary,
                _activityTypeProvider.IntTypeDictionary,
                entity);

        public static PermissionEntity UpdateEntity(PermissionEntity entity, PermissionUpdateModel update)
        {
            entity.IsAllowed = update.SettingValues.IsAllowed;
            entity.IsEnabled = update.SettingValues.IsEnabled;

            return entity;
        }

        public static PermissionEntity CreateEntity(PermissionUpdateModel update) =>
            new PermissionEntity
            {
                Id = Guid.NewGuid(),
                IntranetMemberGroupId = update.Group.Id,
                IntranetActionId = update.Action.ToInt(),
                ActivityTypeId = update.ActivityType.ToNullableInt(),
                IsAllowed = update.SettingValues.IsAllowed,
                IsEnabled = update.SettingValues.IsEnabled
            };

        public static Expression<Func<PermissionEntity, bool>> GroupIs(IntranetMemberGroup group)
        {
            var groupId = group.Id;
            return entity => entity.IntranetMemberGroupId == groupId;
        }

        public static Expression<Func<PermissionEntity, bool>> ActionIs(Enum action)
        {
            var actionTypeId = action.ToInt();
            return entity => entity.IntranetActionId == actionTypeId;
        }

        public static Expression<Func<PermissionEntity, bool>> ActivityTypeIs(Option<Enum> value)
        {
            var activityTypeId = value.ToNullableInt();
            return entity => entity.ActivityTypeId == activityTypeId;
        }
    }
}