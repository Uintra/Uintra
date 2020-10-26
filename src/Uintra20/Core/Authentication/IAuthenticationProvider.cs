using Owin;

namespace Uintra20.Core.Authentication
{
	public interface IAuthenticationProvider
	{
		void ConfigureAuthenticationProvider(IAppBuilder app);
	}
}