namespace Uintra20.Features.User
{
    public interface IIntranetUserService<out T>
    {
        T GetByEmailOrNone(string email);
        T GetByIdOrNone(int id);
    }
}
