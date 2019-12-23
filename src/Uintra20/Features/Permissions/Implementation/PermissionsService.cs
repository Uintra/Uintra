using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Permissions.Sql;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;
using static Uintra20.Infrastructure.Extensions.EnumerableExtensions;

namespace Uintra20.Features.Permissions.Implementation
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
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public PermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IPermissionActionTypeProvider intranetActionProvider,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionResourceTypeProvider resourceTypeProvider,
            ICacheService cacheService,
            IPermissionSettingsSchemaProvider permissionSettingsSchema,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _permissionsRepository = permissionsRepository;
            _intranetActionTypeProvider = intranetActionProvider;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _resourceTypeProvider = resourceTypeProvider;
            _cacheService = cacheService;
            _permissionSettingsSchema = permissionSettingsSchema;
            _intranetMemberService = intranetMemberService;
        }

        #region async

        public async Task<IEnumerable<PermissionModel>> GetAllAsync()
        {
            return await CurrentCacheGetAsync();
        }

        public async Task<IEnumerable<PermissionManagementModel>> GetForGroupAsync(IntranetMemberGroup group)
        {
            var storedPerms = (await GetAllAsync())
                .Where(i => i.Group.Id == group.Id)
                .ToDictionary(permission => permission.SettingIdentity, permission => permission.SettingValues);

            var settings = _permissionSettingsSchema
                .Settings
                .Select(settingSchema =>
                {
                    var settingValues = storedPerms.ItemOrDefault(settingSchema.SettingIdentity) 
                                ?? _permissionSettingsSchema.GetDefault(settingSchema.SettingIdentity);

                    return PermissionManagementModel.Of(group, settingSchema, settingValues);
                });

            return settings;
        }

        public async Task<PermissionModel> SaveAsync(PermissionUpdateModel update)
        {
            var storedEntity = await _permissionsRepository
                .FindAsync(GroupIs(update.Group)
                    .AndAlso(ActionIs(update.SettingIdentity.Action))
                    .AndAlso(ActivityTypeIs(update.SettingIdentity.ResourceType)));

            var actualEntity = await storedEntity.Match(
                async entity =>
                {
                    if (!update.SettingValues.IsAllowed && entity.IsAllowed)
                    {
                        var descendants = _permissionSettingsSchema.GetDescendants(update.SettingIdentity)
                            .Select(i => new PermissionEntity
                            {
                                ActionId = i.Action.ToInt(),
                                IntranetMemberGroupId = update.Group.Id,
                                IsAllowed = false,
                                IsEnabled = _permissionSettingsSchema.DefaultSettingsValues.IsEnabled,
                                ResourceTypeId = i.ResourceType.ToInt()
                            }).ToArray();

                        if (descendants.Any())
                            await _permissionsRepository.UpdatePropertyAsync(descendants, i => i.IsAllowed);
                    }

                    var updatedEntity = UpdateEntity(entity, update);
                    await _permissionsRepository.UpdateAsync(updatedEntity);
                    return updatedEntity;
                },
                async () =>
                {
                    var createdEntity = CreateEntity(update);
                    await _permissionsRepository.AddAsync(createdEntity);
                    return createdEntity;
                });

            var actualMappedEntity = Map(actualEntity);
            CurrentCache = null;

            return actualMappedEntity;
        }

        public async Task SaveAsync(IEnumerable<PermissionUpdateModel> permissions)
        {
            var entities = permissions.Select(CreateEntity);
            await _permissionsRepository.AddAsync(entities);

            CurrentCache = null;
        }

        public async Task DeletePermissionsForMemberGroupAsync(int memberGroupId)
        {
            await _permissionsRepository.DeleteAsync(i => i.IntranetMemberGroupId == memberGroupId);
            CurrentCache = null;
        }

        public async Task<bool> CheckAsync(IIntranetMember member, PermissionSettingIdentity settingsIdentity)
        {
            if (member.Groups == null) return false;

            var permission = (await GetAllAsync()).Where(p => member.Groups.Select(g => g.Id).Contains(p.Group.Id) && p.SettingIdentity.Equals(settingsIdentity));

            var isAllowed = permission
                .ToList()
                .Exists(p => p.SettingValues.IsAllowed);

            return isAllowed;
        }

        public async Task<bool> CheckAsync(PermissionSettingIdentity settingsIdentity)
        {
            //var member = await _intranetMemberService.GetCurrentMemberAsync();
            var member = _intranetMemberService.GetCurrentMember();
            return await CheckAsync(member, settingsIdentity);
        }

        public async Task<bool> CheckAsync(Enum resourceType, Enum actionType)
        {
            //var member = await _intranetMemberService.GetCurrentMemberAsync();
            var member = _intranetMemberService.GetCurrentMember();
            return await CheckAsync(member, PermissionSettingIdentity.Of(actionType, resourceType));
        }

        protected virtual async Task<IEnumerable<PermissionModel>> CurrentCacheGetAsync()
        {
            return await _cacheService.GetOrSetAsync(
                async () =>
                {
                    var permissionEntities = await _permissionsRepository.AsNoTrackingAsync();
                    return MapAll(permissionEntities);
                },
                BasePermissionCacheKey);
        }

        protected virtual async Task CurrentCachSetAsync(IEnumerable<PermissionModel> value)
        {
            if (value == null)
                _cacheService.Remove(BasePermissionCacheKey);
            else
                await _cacheService.SetAsync(() => Task.FromResult(value), BasePermissionCacheKey);
        }

        #endregion

        #region sync

        protected virtual IEnumerable<PermissionModel> CurrentCache
        {
            get => _cacheService.GetOrSet(
                    BasePermissionCacheKey,
                    () => MapAll(_permissionsRepository.AsNoTracking()));
            set
            {
                if (value == null)
                    _cacheService.Remove(BasePermissionCacheKey);
                else
                    _cacheService.Set(BasePermissionCacheKey, value);
            }
        }

        public virtual IEnumerable<PermissionModel> GetAll() =>
            CurrentCache;

        public virtual IEnumerable<PermissionManagementModel> GetForGroup(IntranetMemberGroup group)
        {
            var storedPerms = GetAll()
                .Where(i => i.Group.Id == group.Id)
                .ToDictionary(permission => permission.SettingIdentity, permission => permission.SettingValues);

            var settings = _permissionSettingsSchema.Settings
                .Select(settingSchema =>
                {
                    var settingValues = storedPerms.ItemOrDefault(settingSchema.SettingIdentity)
                                           ?? _permissionSettingsSchema.GetDefault(settingSchema.SettingIdentity);

                    return PermissionManagementModel.Of(@group, settingSchema, settingValues);
                });

            return settings;
        }

        protected virtual IEnumerable<PermissionModel> MapAll(IEnumerable<PermissionEntity> entities) =>
            entities.Select(Map);

        public virtual PermissionModel Save(PermissionUpdateModel update)
        {
            var storedEntity = _permissionsRepository
                .Find(
                    GroupIs(update.Group)
                        .AndAlso(ActionIs(update.SettingIdentity.Action))
                        .AndAlso(ActivityTypeIs(update.SettingIdentity.ResourceType)));

            var actualEntity = storedEntity.Match(
                entity =>
                {
                    if (!update.SettingValues.IsAllowed && entity.IsAllowed)
                    {
                        var descendants = _permissionSettingsSchema.GetDescendants(update.SettingIdentity)
                            .Select(i => new PermissionEntity
                            {
                                ActionId = i.Action.ToInt(),
                                IntranetMemberGroupId = update.Group.Id,
                                IsAllowed = false,
                                IsEnabled = _permissionSettingsSchema.DefaultSettingsValues.IsEnabled,
                                ResourceTypeId = i.ResourceType.ToInt()
                            }).ToArray();

                        if (descendants.Any())
                            _permissionsRepository.UpdateProperty(descendants, i => i.IsAllowed);
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
            CurrentCache = null;
        }

        public virtual bool Check(IIntranetMember member, PermissionSettingIdentity settingIdentity)
        {
            if (member.Groups == null) return false;

            var permission = GetAll()
                .Where(p => member
                                .Groups
                                .Select(g => g.Id)
                                .Contains(p.Group.Id) && p.SettingIdentity.Equals(settingIdentity));

            var isAllowed = permission
                .ToList()
                .Exists(p => p.SettingValues.IsAllowed);

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
                IntranetMemberGroupId = update.Group.Id,
                ActionId = update.SettingIdentity.Action.ToInt(),
                ResourceTypeId = update.SettingIdentity.ResourceType.ToInt(),
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

        #endregion
    }
}