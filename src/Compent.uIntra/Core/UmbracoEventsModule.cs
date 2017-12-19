using System.Web.Mvc;
using uIntra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core
{
    public static class UmbracoEventsModule
    {
        public static void RegisterEvents()
        {
            // TODO: use separate interfaces for publishing, published, saving, saved etc..
            ContentService.Published += Process;
            ContentService.UnPublished += Process;
            ContentService.Trashed += ProcessContentTrashed;

            MemberService.Deleting += Process;
            MediaService.Saved += Process;
            MediaService.Trashed += Process;            
        }

        private static void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentTrashedEventsService>();
            foreach (var service in services) service.ProcessContentTrashed(sender, e);
        }

        private static void Process<TService, TEventArgs>(TService sender, TEventArgs e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoEventService<TService, TEventArgs>>();
            foreach (var service in services) service.Process(sender, e);
        }
    }
}