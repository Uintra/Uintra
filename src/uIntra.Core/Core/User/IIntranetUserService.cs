namespace Uintra.Core.User
{
    public interface IIntranetUserService<out T>
    {
        T AddUser(IIntranetMember member);

        T GetUser(string email);

        T GetUser(int id);
    }
}