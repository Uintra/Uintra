namespace Uintra.Users
{
    public class UserListViewModel
    {
        public int DisplayedAmount { get; set; }
        public int AmountPerRequest { get; set; }
        public string Title { get; set; }
        public UsersRowsViewModel UsersRows { get; set; }
        public string UsersRowsViewPath { get; set; }
    }
}
