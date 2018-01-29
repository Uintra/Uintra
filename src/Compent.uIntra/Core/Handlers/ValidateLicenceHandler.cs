using System;
using System.Web;
using System.Web.Mvc;
using Compent.uIntra.Core.Licence;
using Umbraco.Core;

namespace Compent.uIntra.Core.Handlers
{
#if (!DISABLE_LICENCE)
    public sealed class ValidateLicenceHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

             UmbracoApplicationBase.ApplicationInit += Init;

        }

        private void Init(object sender, EventArgs eventArgs)
        {
            var app = (HttpApplication) sender;
            var licenceRequestHandler = DependencyResolver.Current.GetService<ILicenceRequestHandler>();
            app.BeginRequest += licenceRequestHandler.BeginRequestHandler;
        }
    }
#endif
}