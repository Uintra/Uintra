using System.Linq;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.Sql;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions
{
    public class RoleService : IRoleService
    {
        private readonly ISqlRepository<int, RoleEntity> _roleRepository;

        public RoleService(ISqlRepository<int, RoleEntity> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Role[] GetAll() => _roleRepository
            .GetAll()
            .Select(roleEntity => new Role(roleEntity.Id, roleEntity.Name))
            .ToArray();

        public void Add(string name) => _roleRepository
            .Add(new RoleEntity {Name = name});

        public void Remove(Role role) =>
            _roleRepository.Delete(role.Id);
    }
}
