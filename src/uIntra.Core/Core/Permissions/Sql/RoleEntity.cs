using System.ComponentModel.DataAnnotations;
using Uintra.Core.Persistence;

namespace Uintra.Core.Permissions.Sql
{
    [UintraTable("Role")]
    public class RoleEntity : SqlEntity<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Name { get; set; }
    }
}
