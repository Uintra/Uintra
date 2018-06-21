using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Users
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class UIColumnAttribute : Attribute
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string DisplayName { get; set; }
        public ColumnType Type { get; set; }

        public UIColumnAttribute(int order, string backofficeDisplayName, ColumnType type = ColumnType.Text, string alias = null)
        {
            Id = order;
            DisplayName = backofficeDisplayName;
            Type = type;
            Alias = alias ?? DisplayName?.Replace(" ", string.Empty);
        }
    }
}
