using System.Threading.Tasks;
using Microsoft.Owin;

namespace Uintra20.Core.Authentication
{
	public interface IAuthenticationService
	{
		bool Validate(string login, string password);
		void Login(string login, string password);
        Task Logout();
        bool IsAuthenticatedRequest(IOwinContext context);
    }
}