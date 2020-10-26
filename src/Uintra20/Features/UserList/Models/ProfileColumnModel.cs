namespace Uintra20.Features.UserList.Models
{
    public class ProfileColumnModel
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public string PropertyName { get; set; }
        public bool SupportSorting { get; set; }
    }
}