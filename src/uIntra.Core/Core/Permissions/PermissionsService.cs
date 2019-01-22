using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Compent.Extensions;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.Sql;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.Persistence;
using Uintra.Core.TypeProviders;
using static LanguageExt.Prelude;
using static Uintra.Core.Extensions.EnumerableExtensions;
using EnumExtensions = Uintra.Core.Extensions.EnumExtensions;

namespace Uintra.Core.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly ISqlRepository<PermissionEntity> _permissionsRepository;
        private readonly IPermissionTypeProvider _permissionsTypeProvider;
        private readonly IRoleTypeProvider _roleTypeProvider;

        public PermissionsService(ISqlRepository<PermissionEntity> permissionsRepository, IRoleTypeProvider roleTypeProvider)
        {
            _permissionsRepository = permissionsRepository;
            _roleTypeProvider = roleTypeProvider;
        }

        public EntityPermissions Get(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public EntityRolePermissions Get(Guid entityId, Role role)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var transientEntities = ToTransientEntities(
                queryResult,
                _permissionsTypeProvider.IntTypeDictionary,
                _roleTypeProvider.IntTypeDictionary);

            var entityRolePermissions = ToModel(entityId, role, transientEntities);

            return entityRolePermissions;
        }

        public bool Has(Guid entityId, Role role, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id), PermissionIdIn(permissions));
            var queryResult = _permissionsRepository.Count(predicate);

            return queryResult == permissions.Length;
        }

        public Unit Save(EntityRolePermissions permissions)
        {
            var predicate = AndAlso(EntityIdIs(permissions.EntityId), RoleIdIs(permissions.Role.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var (permissionsToAdd, permissionsToDelete) = PermissionsDifference(queryResult, permissions);

            _permissionsRepository.Delete(permissionsToDelete);

            throw new NotImplementedException();
        }

        public Unit Add(Guid entityId, Role role, Enum[] permissions)
        {
            throw new NotImplementedException();
        }

        public Unit Remove(Guid entityId, Role role, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id), PermissionIdIn(permissions));
            _permissionsRepository.Delete(predicate);

            return unit;
        }


        public static PermissionEntity Create(Guid entityId, Role role, int permissionType) =>
            new PermissionEntity
            {
                EntityId = entityId,
                RoleType = role.Id,
                PermissionType = permissionType
            };

        public static (IEnumerable<int> permissionsToAdd, IEnumerable<PermissionEntity> permissionsToDelete) PermissionsDifference(
            IList<PermissionEntity> storedEntities,
            EntityRolePermissions proposedPermission)
        {
            var(permissionsToAdd, permissionsToDelete) = storedEntities
                .Select(x => x.RoleType)
                .Difference(proposedPermission.Permissions.Select(EnumExtensions.ToInt));

            var permissionEntitiesToRemove = storedEntities.Where(e => permissionsToDelete.Contains(e.PermissionType));

            return (permissionsToAdd, permissionEntitiesToRemove);
        }

        public static Expression<Func<PermissionEntity, bool>> EntityIdIs(Guid id) => 
            entity => entity.EntityId == id;

        public static Expression<Func<PermissionEntity, bool>> RoleIdIs(int id) =>
            entity => entity.RoleType == id;

        public static Expression<Func<PermissionEntity, bool>> PermissionIdIn(params Enum[] permissions)
        {
            var permissionIds = permissions.Select(EnumExtensions.ToInt);
            return entity => permissionIds.Contains(entity.PermissionType);
        }


        public static IEnumerable<TransientPermissionEntity> ToTransientEntities(
            IList<PermissionEntity> entities,
            IDictionary<int, Enum> permissionTypeDictionary,
            IDictionary<int, Role> roleTypeDictionary
        ) =>
            entities.Select(e => new TransientPermissionEntity
            {
                Id = e.Id,
                EntityId = e.EntityId,
                PermissionType = permissionTypeDictionary[e.PermissionType],
                RoleType = roleTypeDictionary[e.RoleType]
            });

        public static EntityRolePermissions ToModel(
            Guid entityId,
            Role role,
            IEnumerable<TransientPermissionEntity> entities) =>
            new EntityRolePermissions(entityId, role, entities.Select(entity => entity.PermissionType).ToArray());
    }
}