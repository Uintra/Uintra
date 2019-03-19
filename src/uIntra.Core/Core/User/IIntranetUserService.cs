using LanguageExt;

namespace Uintra.Core.User
{
    public interface IIntranetUserService<T>
    {
        Option<T> GetByEmailOrNone(string email);
        Option<T> GetByIdOrNone(int id);
    }
}