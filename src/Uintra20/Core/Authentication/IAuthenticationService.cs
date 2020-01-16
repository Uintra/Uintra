using Microsoft.Owin;
using System.Threading.Tasks;

namespace Uintra20.Core.Authentication
{
	public interface IAuthenticationService
	{
		bool Validate(string login, string password);
		Task LoginAsync(string login, string password);
        bool Logout();
        bool IsAuthenticatedRequest(IOwinContext context);
    }
}