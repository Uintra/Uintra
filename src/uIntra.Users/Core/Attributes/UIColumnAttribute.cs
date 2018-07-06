using System;
using Uintra.Users.UserList;

namespace Uintra.Users.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class UIColumnAttribute : Attribute
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string DisplayName { get; set; }
        public ColumnType Type { get; set; }
        public string PropertyName { get; set; }

        public UIColumnAttribute(int order, string backofficeDisplayName, string propertyName, ColumnType type = ColumnType.Text, string alias = null)
        {
            Id = order;
            DisplayName = backofficeDisplayName;
            Type = type;
            PropertyName = propertyName;
            Alias = alias ?? DisplayName?.Replace(" ", string.Empty);
        }
    }
}
