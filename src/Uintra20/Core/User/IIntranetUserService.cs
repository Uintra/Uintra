using System.Threading.Tasks;

namespace Uintra20.Core.User
{
    public interface IIntranetUserService<T>
    {
	    T GetByEmail(string email);
	    T GetById(int id);
		Task<T> GetByEmailAsync(string email);
        Task<T> GetByIdAsync(int id);
	}
}
