using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Compent.uCommunity.Startup))]
namespace Compent.uCommunity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
