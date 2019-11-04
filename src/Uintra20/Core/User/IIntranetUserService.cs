namespace Uintra20.Core.User
{
    public interface IIntranetUserService<out T>
    {
        T GetByEmailOrNone(string email);
        T GetByIdOrNone(int id);
    }
}
