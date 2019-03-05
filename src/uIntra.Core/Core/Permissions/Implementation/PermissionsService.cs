using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
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
        private readonly IPermissionResourceTypeProvider _resourceTypeProvider;
        private readonly ICacheService _cacheService;
        private readonly IPermissionSettingsSchemaProvider _permissionSettingsSchema;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public PermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IPermissionActionTypeProvider intranetActionProvider,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionResourceTypeProvider resourceTypeProvider,
            ICacheService cacheService,
            IPermissionSettingsSchemaProvider permissionSettingsSchema,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _permissionsRepository = permissionsRepository;
            _intranetActionTypeProvider = intranetActionProvider;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _resourceTypeProvider = resourceTypeProvider;
            _cacheService = cacheService;
            _permissionSettingsSchema = permissionSettingsSchema;
            _intranetMemberService = intranetMemberService;
        }

        protected virtual IEnumerable<PermissionModel> CurrentCache
        {
            get => _cacheService.GetOrSet(
                    BasePermissionCacheKey,
                    () => _permissionsRepository.AsNoTracking().Apply(MapAll));
            
            set
            {
                if (value == null)
                    _cacheService.Remove(BasePermissionCacheKey);
                else
                    _cacheService.Set(BasePermissionCacheKey, value);
            }

            //get => _permissionsRepository.AsNoTracking().Apply(MapAll);
            //set { }
        }

        public virtual IEnumerable<PermissionModel> GetAll()
        {
            return CurrentCache;
        }

        public virtual IEnumerable<PermissionManagementModel> GetForGroup(IntranetMemberGroup group)
        {
            var storedPerms = GetAll()
                .Where(i => i.Group.Id == group.Id)
                .ToDictionary(permission => permission.SettingIdentity, permission => permission.SettingValues);

            var settings = _permissionSettingsSchema.Settings
                .Select(settingSchema => storedPerms
                    .ItemOrNone(settingSchema.SettingIdentity)
                    .IfNone(() => _permissionSettingsSchema.GetDefault(settingSchema.SettingIdentity))
                    .Apply(settingValues => PermissionManagementModel.Of(group, settingSchema, settingValues)));

            return settings;
        }

        protected virtual IEnumerable<PermissionModel> MapAll(IEnumerable<PermissionEntity> entities) =>
            entities.Select(Map);

        public virtual PermissionModel Save(PermissionUpdateModel update)
        {
            var settings = _permissionSettingsSchema.Settings;

            IEnumerable<PermissionSettingIdentity> GetChildren(IEnumerable<Enum> actionIds)
            {
                var children = settings
                    .Where(i => i.ParentActionType.Some(j => actionIds.Contains(j)).None(false) &&
                        i.SettingIdentity.ResourceType.Equals(update.ResourceType))
                    .Select(i => i.SettingIdentity);
                if (children.Any())
                    children = children.Append(GetChildren(children.Select(i => i.Action)));
                return children;
            }

            var storedEntity = _permissionsRepository
                .FindOrNone(AndAlso(GroupIs(update.Group),
                    ActionIs(update.Action),
                    ActivityTypeIs(update.ResourceType)));

            var actualEntity = storedEntity.Match(
                entity =>
                {
                    if (update.SettingValues.IsAllowed == false && entity.IsAllowed == true)
                    {
                        var children = GetChildren(update.Action.ToListOfOne())
                            .Select(i => new PermissionEntity()
                            {
                                ActionId = i.Action.ToInt(),
                                IntranetMemberGroupId = update.Group.Id,
                                IsAllowed = false,
                                ResourceTypeId = i.ResourceType.ToInt()
                            });
                        if (children.Any())
                            _permissionsRepository.UpdateProperty(children, i => i.IsAllowed);
                    }

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
            //CurrentCache = CurrentCache.WithUpdatedElement(e => e.Id, actualMappedEntity);
            CurrentCache = null;

            return actualMappedEntity;
        }

        public virtual void Save(IEnumerable<PermissionUpdateModel> permissions)
        {
            var entities = permissions.Select(CreateEntity);
            _permissionsRepository.Add(entities);
            CurrentCache = null;
        }

        public virtual void DeletePermissionsForMemberGroup(int memberGroupId)
        {
            _permissionsRepository.Delete(i => i.IntranetMemberGroupId == memberGroupId);
            //CurrentCache = CurrentCache.Where(i => i.Group.Id != memberGroupId);
            CurrentCache = null;
        }

        public virtual bool Check(IIntranetMember member, PermissionSettingIdentity settingIdentity)
        {
            if (member.Group == null) return false;

            var permission = GetAll().Find(p => p.Group.Id == member.Group.Id && p.SettingIdentity.Equals(settingIdentity));

            var isAllowed = permission.Match(p => p.SettingValues.IsAllowed, () => false);

            return isAllowed;
        }

        public virtual bool Check(PermissionSettingIdentity settingIdentity)
        {
            var member = _intranetMemberService.GetCurrentMember();
            return Check(member, settingIdentity);
        }

        public virtual bool Check(Enum resourceType, Enum action)
        {
            var member = _intranetMemberService.GetCurrentMember();
            return Check(member, PermissionSettingIdentity.Of(action, resourceType));
        }

        /*protected virtual PermissionModel Map(PermissionEntity entity) =>
            PermissionModel.Of(
                _intranetMemberGroupProvider.IntTypeDictionary,
                _intranetActionTypeProvider.IntTypeDictionary,
                _activityTypeProvider.IntTypeDictionary,
                entity);*/

        protected virtual PermissionModel Map(PermissionEntity entity) =>
            PermissionModel.Of(
                PermissionSettingIdentity.Of(
                    _intranetActionTypeProvider[entity.ActionId],
                    _resourceTypeProvider[entity.ResourceTypeId]
                ),
                PermissionSettingValues.Of(
                    entity.IsAllowed,
                    entity.IsEnabled
                ),
                _intranetMemberGroupProvider[entity.IntranetMemberGroupId]);

        public static PermissionEntity UpdateEntity(PermissionEntity entity, PermissionUpdateModel update)
        {
            entity.IsAllowed = update.SettingValues.IsAllowed;
            entity.IsEnabled = update.SettingValues.IsEnabled;

            return entity;
        }

        public static PermissionEntity CreateEntity(PermissionUpdateModel update) =>
            new PermissionEntity
            {
                //d = Guid.NewGuid(),
                IntranetMemberGroupId = update.Group.Id,
                ActionId = update.Action.ToInt(),
                ResourceTypeId = update.ResourceType.ToInt(),
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
            return entity => entity.ActionId == actionTypeId;
        }

        public static Expression<Func<PermissionEntity, bool>> ActivityTypeIs(Enum value)
        {
            var resourceTypeId = value.ToInt();
            return entity => entity.ResourceTypeId == resourceTypeId;
        }
    }
}