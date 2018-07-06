namespace Uintra.Users.UserList
{
    public class UserListModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public dynamic SelectedProperties { get; set; }
        public dynamic OrderBy { get; set; }
    }
}
