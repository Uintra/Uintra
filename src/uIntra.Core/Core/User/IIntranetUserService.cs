using LanguageExt;

namespace Uintra.Core.User
{
    public interface IIntranetUserService<out T>
    {
        T GetByEmailOrNone(string email);
        T GetByIdOrNone(int id);
    }
}