using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.Sql;
using Uintra.Core.Persistence;
using Uintra.Core.TypeProviders;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class ActivityTypePermissionsService : IActivityTypePermissionsService
    {
        protected virtual string BasePermissionCacheKey => "BasePermissionCache";

        private readonly ISqlRepository<PermissionActivityTypeEntity> _activityTypePermissionRepository;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly ICacheService _cacheService;

        public ActivityTypePermissionsService(
            ISqlRepository<PermissionActivityTypeEntity> activityTypePermissionRepository,
            IActivityTypeProvider activityTypeProvider,
            ICacheService cacheService)
        {
            _activityTypePermissionRepository = activityTypePermissionRepository;
            _activityTypeProvider = activityTypeProvider;
            _cacheService = cacheService;
        }

        public Unit Save(ActivityTypePermissionCreateModel createInfo)
        {
            var storedEntity = _activityTypePermissionRepository
                .FindOrNone(i => i.ActivityTypeId == (int)(object)createInfo.ActivityType &&
                    i.PermissionEntityId == createInfo.BasePermissionId);
            var actualEntity = storedEntity.Match(
                entity =>
                { },
                () =>
                {
                    var createdEntity = CreateEntity(createInfo);
                    _activityTypePermissionRepository.Add(createdEntity);
                    var actualMappedEntity = Map(createdEntity);

                    var oldCache = _cacheService.Get<IReadOnlyList<ActivityTypePermissionModel>>(BasePermissionCacheKey) ??
                        new List<ActivityTypePermissionModel>().AsReadOnly();

                    _cacheService.Set(
                        BasePermissionCacheKey,
                        oldCache.WithUpdatedElement(e => e.Id, actualMappedEntity));
                }
                );

            return unit;
        }

        public IEnumerable<ActivityTypePermissionModel> GetAll()
        {
            var entities = _activityTypePermissionRepository.GetAll();

            var models = entities.Select(Map);

            return models;
        }

        public ActivityTypePermissionModel Map(PermissionActivityTypeEntity entity) =>
            ActivityTypePermissionModel.Of(
                _activityTypeProvider.IntTypeDictionary,
                entity);

        public static PermissionActivityTypeEntity CreateEntity(ActivityTypePermissionCreateModel creationInfo) =>
            new PermissionActivityTypeEntity
            {
                Id = Guid.NewGuid(),
                PermissionEntityId = creationInfo.BasePermissionId,
                ActivityTypeId = creationInfo.ActivityType.ToInt()
            };
    }
}