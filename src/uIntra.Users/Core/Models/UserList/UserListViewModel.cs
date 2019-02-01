namespace Uintra.Users.UserList
{
    public class UserListViewModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public UsersRowsViewModel UsersRows { get; set; }
        public ProfileColumnModel OrderByColumn { get; set; }
        public bool IsLastRequest { get; set; }
    }
}
