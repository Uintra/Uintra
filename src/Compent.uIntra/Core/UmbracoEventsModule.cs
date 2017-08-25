using System.Web.Mvc;
using uIntra.Core.User;
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

            MemberService.Saved += MemberServiceOnSaved;
            MemberService.Deleted += MemberServiceOnDeleted;
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

        private static void MemberServiceOnSaved(IMemberService sender, SaveEventArgs<IMember> e)
        {
            var cacheableUserService = GetCacheableUserService();

            foreach (var member in e.SavedEntities)
            {
                cacheableUserService?.UpdateUserCache(member.Key);
            }
        }

        private static void MemberServiceOnDeleted(IMemberService sender, DeleteEventArgs<IMember> e)
        {
            var cacheableUserService = GetCacheableUserService();

            foreach (var member in e.DeletedEntities)
            {
                cacheableUserService?.UpdateUserCache(member.Key);
            }
        }

        private static ICacheableIntranetUserService GetCacheableUserService()
        {
            var userService = DependencyResolver.Current.GetService<IIntranetUserService<IIntranetUser>>();
            var cacheableUserService = userService as ICacheableIntranetUserService;

            return cacheableUserService;
        }

    }
}