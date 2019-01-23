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

        public PermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IRoleTypeProvider roleTypeProvider,
            IPermissionTypeProvider permissionsTypeProvider)
        {
            _permissionsRepository = permissionsRepository;
            _roleTypeProvider = roleTypeProvider;
            _permissionsTypeProvider = permissionsTypeProvider;
        }

        public EntityPermissions Get(Guid entityId)
        {
            var predicate = EntityIdIs(entityId);
            var queryResult = _permissionsRepository.FindAll(predicate);

            var (rolesByEntityId, permissionsByEntityIdAndRole) = ToTransientEntities(queryResult,
                    _permissionsTypeProvider.IntTypeDictionary, _roleTypeProvider.IntTypeDictionary)
                .Apply(ToDictionaries);

            var result = ToModel(entityId, rolesByEntityId, permissionsByEntityIdAndRole);

            return result;
        }

        public EntityRolePermissions GeForRole(Guid entityId, Role role)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var (_, permissionsByEntityIdAndRole) = ToTransientEntities(queryResult,
                    _permissionsTypeProvider.IntTypeDictionary, _roleTypeProvider.IntTypeDictionary)
                .Apply(ToDictionaries);

            var result = ToModel(entityId, role, permissionsByEntityIdAndRole[(entityId, role)]);

            return result;
        }

        public bool Has(Guid entityId, Role role, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id), PermissionIdIn(permissions));
            var queryResult = _permissionsRepository.Count(predicate);

            return queryResult == permissions.Length;
        }

        public Unit Save(EntityPermissions permissions)
        {
            var predicate = AndAlso(EntityIdIs(permissions.EntityId));
           
            var permissionsToAdd = ToEntities(permissions);

            _permissionsRepository.Delete(predicate);
            _permissionsRepository.Add(permissionsToAdd);

            return unit;
        }

        public Unit Save(EntityRolePermissions permissions)
        {
            var predicate = AndAlso(EntityIdIs(permissions.EntityId), RoleIdIs(permissions.Role.Id));
            _permissionsRepository.Delete(predicate);

            var permissionsToAdd = ToEntities(permissions);

            _permissionsRepository.Add(permissionsToAdd);

            return unit;
        }

        public Unit Add(Guid entityId, Role role, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(role.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var permissionsToAdd = PermissionEntitiesToAdd(entityId, role, queryResult, permissions);

            _permissionsRepository.Add(permissionsToAdd);

            return unit;
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

        public static IEnumerable<PermissionEntity> PermissionEntitiesToAdd(
            Guid entityId,
            Role role,
            IList<PermissionEntity> storedEntities,
            Enum[] proposedPermissions)
        {
            var (_, missingPermissions) = storedEntities
                .Select(x => x.RoleType)
                .Difference(proposedPermissions.Select(EnumExtensions.ToInt));

            var permissionEntitiesToAdd = missingPermissions
                .Select(permission => Create(entityId, role, permission));

            return permissionEntitiesToAdd;
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


        public static (Dictionary<Guid, IEnumerable<Role>>, Dictionary<(Guid, Role), IEnumerable<Enum>>) ToDictionaries(
            IEnumerable<TransientPermissionEntity> entities
        )
        {
            var entitiesArray = entities.ToArray();

            var rolesByEntityId = entitiesArray
                .ToLookup(e => e.EntityId, e => e.RoleType)
                .ToDictionary(pair => pair.Key, Enumerable.AsEnumerable);

            var permissionsByEntityIdAndRole = entitiesArray
                .ToLookup(e => (e.EntityId, e.RoleType), e => e.PermissionType)
                .ToDictionary(pair => pair.Key, Enumerable.AsEnumerable);

            return (rolesByEntityId, permissionsByEntityIdAndRole);
        }

        public static EntityRolePermissions ToModel(Guid entityId, Role role, IEnumerable<Enum> permissions) =>
            new EntityRolePermissions(entityId, role, permissions.ToArray());

        public static EntityPermissions ToModel(
            Guid entityId,
            Dictionary<Guid, IEnumerable<Role>> rolesByEntityId,
            Dictionary<(Guid, Role), IEnumerable<Enum>> permissionsByEntityIdAndRole)
        {
            var entityRoles = rolesByEntityId[entityId];
            var permissionsByRole = entityRoles
                .ToDictionary(identity, role => permissionsByEntityIdAndRole[(entityId, role)].ToArray());

            return new EntityPermissions(entityId, permissionsByRole);
        }
        

        public static IEnumerable<PermissionEntity> ToEntities(EntityPermissions permissions)=>
            permissions.Permissions
                .SelectMany(permissionsByRole =>
                    permissionsByRole.Value
                        .Select(permission => 
                            Create(permissions.EntityId, permissionsByRole.Key, permission.ToInt())));

        public static IEnumerable<PermissionEntity> ToEntities(EntityRolePermissions permissions) =>
            permissions.Permissions
                        .Select(permission =>
                            Create(permissions.EntityId, permissions.Role, permission.ToInt()));
    }
}