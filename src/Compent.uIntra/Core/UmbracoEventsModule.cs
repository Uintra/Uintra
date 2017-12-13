using System.Web.Mvc;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core
{
    public static class UmbracoEventsModule
    {
        public static void RegisterEvents()
        {
            ContentService.Published += Process;
            ContentService.UnPublished += Process;
            MemberService.Deleting += Process;
            MediaService.Saved += Process;
            MediaService.Trashed += Process;
        }

        private static void Process<TService, TEventArgs>(TService sender, TEventArgs e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoEventService<TService, TEventArgs>>();
            foreach (var service in services) service.Process(sender, e);
        }
    }
}