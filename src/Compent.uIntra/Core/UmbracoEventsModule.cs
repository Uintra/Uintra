using System.Web.Mvc;
using uIntra.Search;
using uIntra.Users;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core
{
    public static class UmbracoEventsModule
    {
        private static IContentIndexer _contentIndexer;

        public static void RegisterEvents()
        {
            Init();
            ContentService.Published += ContentServiceOnPublished;
            ContentService.UnPublished += ContentServiceOnUnPublished;
            MemberService.Deleting += MemberServiceOnDeleting;
        }

        private static void Init()
        {
            _contentIndexer = DependencyResolver.Current.GetService<IContentIndexer>();
        }

        private static void ContentServiceOnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                _contentIndexer.FillIndex(entity.Id);
            }
        }

        private static void ContentServiceOnUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> publishEventArgs)
        {
            foreach (var entity in publishEventArgs.PublishedEntities)
            {
                _contentIndexer.DeleteFromIndex(entity.Id);
            }
        }

        private static void MemberServiceOnDeleting(IMemberService sender, DeleteEventArgs<IMember> e)
        {
            var cacheableUserService = DependencyResolver.Current.GetService<ICacheableIntranetUserService>();
            var memberService = DependencyResolver.Current.GetService<IMemberService>();

            foreach (var member in e.DeletedEntities)
            {
                member.IsLockedOut = true;
                memberService?.Save(member);
                cacheableUserService?.UpdateUserCache(member.Key);
            }

            if (e.CanCancel)
            {
                e.Cancel = true;                           
            }
        }
    }
}