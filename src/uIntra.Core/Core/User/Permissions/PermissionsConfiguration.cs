using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace uIntra.Core.User.Permissions
{
    public class PermissionsConfiguration : ConfigurationSection, IPermissionsConfiguration
    {
        public static PermissionsConfiguration Configure => ConfigurationManager.GetSection("userPermissions") as PermissionsConfiguration;

        [ConfigurationProperty("roles", IsRequired = true)]
        public RolesCollection Roles => (RolesCollection)base["roles"];
    }

    [ConfigurationCollection(typeof(Role), AddItemName = "role")]
    public class RolesCollection : ConfigurationElementCollection, IEnumerable<Role>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Role();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as Role;
            return configElement?.Key;
        }

        IEnumerator<Role> IEnumerable<Role>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, Count) select this[i]).GetEnumerator();
        }

        public Role this[int index] => BaseGet(index) as Role;
    }

    public class Role: ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key => (string)base["key"];

        [ConfigurationProperty("permissions")]
        public PermissionsCollection Permissions => (PermissionsCollection)base["permissions"];
    }

    [ConfigurationCollection(typeof(Permission), AddItemName = "permission")]
    public class PermissionsCollection : ConfigurationElementCollection, IEnumerable<Permission>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Permission();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as Permission;
            return configElement?.Key;
        }

        IEnumerator<Permission> IEnumerable<Permission>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, Count) select this[i]).GetEnumerator();
        }

        public Permission this[int index] => BaseGet(index) as Permission;
    }

    public class Permission : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key => (string)base["key"];
    }
}