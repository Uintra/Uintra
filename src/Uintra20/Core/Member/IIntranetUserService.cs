namespace Uintra20.Core.Member
{
    public interface IIntranetUserService<out T>
    {
        T GetByEmailOrNone(string email);
        T GetByIdOrNone(int id);
    }
}
