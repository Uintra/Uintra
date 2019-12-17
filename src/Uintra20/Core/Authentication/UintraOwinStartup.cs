using Microsoft.Owin;
using Owin;
using Uintra20.Core.Authentication;
using Umbraco.Web;

[assembly: OwinStartup("UintraOwinStartup", typeof(UintraOwinStartup))]
namespace Uintra20.Core.Authentication
{
	
	public class UintraOwinStartup : UmbracoDefaultOwinStartup
	{
		public override void Configuration(IAppBuilder app)
		{
			base.Configuration(app);
			var provider = new AuthenticationConfigurator(new CookieAuthenticationProvider());
			provider.ConfigureAuthenticationStrategy(app);
		}
	}
}