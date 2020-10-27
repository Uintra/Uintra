using Owin;

namespace Uintra.Core.Authentication
{
	public interface IAuthenticationProvider
	{
		void ConfigureAuthenticationProvider(IAppBuilder app);
	}
}