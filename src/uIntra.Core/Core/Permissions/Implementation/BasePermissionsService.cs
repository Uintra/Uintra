using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Uintra.Core.Extensions.EnumerableExtensions;

namespace Uintra.Core.Permissions.Implementation
{
    public class BasePermissionsService : IBasePermissionsService
    {
        protected virtual string BasePermissionCacheKey => "BasePermissionCache";

        private readonly ISqlRepository<PermissionEntity> _permissionsRepository;
        private readonly IIntranetActionTypeProvider _intranetActionTypeProvider;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly ICacheService _cacheService;

        public BasePermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IIntranetActionTypeProvider intranetActionProvider,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            ICacheService cacheService)
        {
            _permissionsRepository = permissionsRepository;
            _intranetActionTypeProvider = intranetActionProvider;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _cacheService = cacheService;
        }

        public IReadOnlyCollection<BasePermissionModel> GetAll()
        {
            return _cacheService.GetOrSet(
                BasePermissionCacheKey,
                () => _permissionsRepository.GetAll().Apply(MapAll));
        }

        private ReadOnlyCollection<BasePermissionModel> MapAll(IEnumerable<PermissionEntity> entities) =>
            entities.Select(Map).ToList().AsReadOnly();

        public BasePermissionModel Save(BasePermissionUpdateModel update)
        {
            var storedEntity = _permissionsRepository
                //.FindOrNone(AndAlso(GroupIs(update.Group), ActionIs(update.Action)));
                .FindOrNone(i => i.IntranetMemberGroupId == update.Group.Id &&
                    i.IntranetActionId == (int)(object)update.Action);

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
            var oldCache = _cacheService.Get<IReadOnlyList<BasePermissionModel>>(BasePermissionCacheKey) ??
                new List<BasePermissionModel>().AsReadOnly();

            _cacheService.Set(BasePermissionCacheKey, oldCache.WithUpdatedElement(e => e.Id, actualMappedEntity));

            return actualMappedEntity;
        }

        protected BasePermissionModel Map(PermissionEntity entity) =>
            BasePermissionModel.Of(
                _intranetMemberGroupProvider.IntTypeDictionary,
                _intranetActionTypeProvider.IntTypeDictionary,
                entity);

        public static PermissionEntity UpdateEntity(PermissionEntity entity, BasePermissionUpdateModel update)
        {
            entity.IntranetMemberGroupId = update.Group.Id;
            entity.IntranetActionId = update.Action.ToInt();
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
            var actionId = action.ToInt();
            return entity => entity.IntranetMemberGroupId == actionId;
        }
    }
}