namespace Uintra.Core.Permissions.Models
{
    public struct Role
    {
        public int Id { get; }
        public string RoleName { get; }

        public Role(int id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }
    }
}
