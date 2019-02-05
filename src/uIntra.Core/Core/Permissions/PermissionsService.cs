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
using static LanguageExt.Prelude;
using static Uintra.Core.Extensions.EnumerableExtensions;
using EnumExtensions = Uintra.Core.Extensions.EnumExtensions;

namespace Uintra.Core.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly ISqlRepository<PermissionEntity> _permissionsRepository;
        private readonly IPermissionTypeProvider _permissionsTypeProvider;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;

        public PermissionsService(
            ISqlRepository<PermissionEntity> permissionsRepository,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionTypeProvider permissionsTypeProvider)
        {
            _permissionsRepository = permissionsRepository;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsTypeProvider = permissionsTypeProvider;
        }

        public EntityPermissions Get(Guid entityId)
        {
            var predicate = EntityIdIs(entityId);
            var queryResult = _permissionsRepository.FindAll(predicate);

            var (groupsByEntityId, permissionsByEntityIdAndRole) = ToTransientEntities(queryResult,
                    _permissionsTypeProvider.IntTypeDictionary, _intranetMemberGroupProvider.IntTypeDictionary)
                .Apply(ToDictionaries);

            var result = ToModel(entityId, groupsByEntityId, permissionsByEntityIdAndRole);

            return result;
        }

        public EntityGroupPermissions GetForGroup(int entityId, IntranetMemberGroup group)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(group.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var (_, permissionsByEntityIdAndRole) = ToTransientEntities(queryResult,
                    _permissionsTypeProvider.IntTypeDictionary, _intranetMemberGroupProvider.IntTypeDictionary)
                .Apply(ToDictionaries);

            var result = ToModel(entityId, group, permissionsByEntityIdAndRole[(entityId, group)]);

            return result;
        }

        public bool Has(Guid entityId, IntranetMemberGroup group, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(group.Id), PermissionIdIn(permissions));
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

        public Unit Save(EntityGroupPermissions permissions)
        {
            var predicate = AndAlso(EntityIdIs(permissions.EntityId), RoleIdIs(permissions.Group.Id));
            _permissionsRepository.Delete(predicate);

            var permissionsToAdd = ToEntities(permissions);

            _permissionsRepository.Add(permissionsToAdd);

            return unit;
        }

        public Unit Add(Guid entityId, IntranetMemberGroup group, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(group.Id));
            var queryResult = _permissionsRepository.FindAll(predicate);

            var permissionsToAdd = PermissionEntitiesToAdd(entityId, group, queryResult, permissions);

            _permissionsRepository.Add(permissionsToAdd);

            return unit;
        }

        public Unit Remove(Guid entityId, IntranetMemberGroup group, Enum[] permissions)
        {
            var predicate = AndAlso(EntityIdIs(entityId), RoleIdIs(group.Id), PermissionIdIn(permissions));
            _permissionsRepository.Delete(predicate);

            return unit;
        }

        public static PermissionEntity Create(Guid entityId, IntranetMemberGroup group, int permissionType) =>
            new PermissionEntity
            {
                EntityId = entityId,
                IntranetMemberGroupId = group.Id,
                ActionId = permissionType
            };

        public static IEnumerable<PermissionEntity> PermissionEntitiesToAdd(
            Guid entityId,
            IntranetMemberGroup group,
            IList<PermissionEntity> storedEntities,
            Enum[] proposedPermissions)
        {
            var (_, missingPermissions) = storedEntities
                .Select(x => x.IntranetMemberGroupId)
                .Difference(proposedPermissions.Select(EnumExtensions.ToInt));

            var permissionEntitiesToAdd = missingPermissions
                .Select(permission => Create(entityId, group, permission));

            return permissionEntitiesToAdd;
        }

        public static Expression<Func<PermissionEntity, bool>> EntityIdIs(Guid id) =>
            entity => entity.EntityId == id;

        public static Expression<Func<PermissionEntity, bool>> RoleIdIs(int id) =>
            entity => entity.IntranetMemberGroupId == id;

        public static Expression<Func<PermissionEntity, bool>> PermissionIdIn(params Enum[] permissions)
        {
            var permissionIds = permissions.Select(EnumExtensions.ToInt);
            return entity => permissionIds.Contains(entity.ActionId);
        }

        public static IEnumerable<TransientPermissionEntity> ToTransientEntities(
            IList<PermissionEntity> entities,
            IDictionary<int, Enum> permissionTypeDictionary,
            IDictionary<int, IntranetMemberGroup> roleTypeDictionary
        ) =>
            entities.Select(e => new TransientPermissionEntity
            {
                Id = e.Id,
                EntityId = e.EntityId,
                PermissionType = permissionTypeDictionary[e.ActionId],
                Group = roleTypeDictionary[e.IntranetMemberGroupId]
            });


        public static (
            Dictionary<Guid, IEnumerable<IntranetMemberGroup>>,
            Dictionary<(Guid, IntranetMemberGroup), IEnumerable<Enum>>
            ) ToDictionaries(
            IEnumerable<TransientPermissionEntity> entities
        )
        {
            var entitiesArray = entities.ToArray();

            var groupsByEntityId = entitiesArray
                .ToLookup(e => e.EntityId, e => e.Group)
                .ToDictionary(pair => pair.Key, Enumerable.AsEnumerable);

            var permissionsByEntityIdAndGroup = entitiesArray
                .ToLookup(e => (e.EntityId, RoleType: e.Group), e => e.PermissionType)
                .ToDictionary(pair => pair.Key, Enumerable.AsEnumerable);

            return (groupsByEntityId, permissionsByEntityIdAndGroup);
        }

        public static EntityGroupPermissions ToModel(Guid entityId, IntranetMemberGroup group, IEnumerable<Enum> permissions) =>
            new EntityGroupPermissions(entityId, group, permissions.ToArray());

        public static EntityPermissions ToModel(
            Guid entityId,
            Dictionary<Guid, IEnumerable<IntranetMemberGroup>> groupsByEntityId,
            Dictionary<(Guid, IntranetMemberGroup), IEnumerable<Enum>> permissionsByEntityIdAndRole)
        {
            var entityRoles = groupsByEntityId[entityId];
            var permissionsByGroup = entityRoles
                .ToDictionary(identity, group => permissionsByEntityIdAndRole[(entityId, group)].ToArray());

            return new EntityPermissions(entityId, permissionsByGroup);
        }
        

        public static IEnumerable<PermissionEntity> ToEntities(EntityPermissions permissions)=>
            permissions.Permissions
                .SelectMany(permissionsByRole =>
                    permissionsByRole.Value
                        .Select(permission => 
                            Create(permissions.EntityId, permissionsByRole.Key, permission.ToInt())));

        public static IEnumerable<PermissionEntity> ToEntities(EntityGroupPermissions permissions) =>
            permissions.Actions
                        .Select(permission =>
                            Create(permissions.EntityId, permissions.Group, permission.ToInt()));
    }
}