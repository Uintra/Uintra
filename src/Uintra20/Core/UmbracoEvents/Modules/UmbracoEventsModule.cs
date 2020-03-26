using System.Web.Mvc;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Entities;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace Uintra20.Core.UmbracoEvents.Modules
{
    public static class UmbracoEventsModule
    {
        public static void RegisterEvents()
        {
            MemberService.AssignedRoles += AssignedRolesHandler;
            MemberService.RemovedRoles += MemberRemovedRolesHandler;
            MemberService.Deleting += MemberDeletingHandler;
            MemberService.Saved += MemberUpdateHandler;
            MemberService.Saving += MemberCreateHandler;
            MemberGroupService.Deleting += MemberGroupDeletingHandler;
            MemberGroupService.Saved += MemberGroupSavedHandler;

            MediaService.Saved += ProcessMediaSaved;
            MediaService.Trashed += ProcessMediaTrashed;
            MediaService.Saving += ProcessMediaSaving;

            ContentService.Published += ProcessContentPublished;
            ContentService.Unpublished += ProcessContentUnPublished;
            ContentService.Trashed += ProcessContentTrashed;
        }

        private static void MemberCreateHandler(IMemberService sender, SaveEventArgs<IMember> e)
        {
            foreach (var entity in e.SavedEntities)
            {
                var dirty = (IRememberBeingDirty)entity;
                var isNew = dirty.WasPropertyDirty("Id");
                if (isNew)
                {
                    var services = DependencyResolver.Current.GetServices<IUmbracoMemberCreatedEventService>();
                    foreach (var service in services)
                    {
                        service.MemberCreateHandler(sender, e);
                    }       
                }
            }
        }

        private static void MemberUpdateHandler(IMemberService sender, SaveEventArgs<IMember> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberCreatedEventService>();

            foreach (var service in services)
            {
                service.MemberUpdateHandler(sender, e);
            }
        }

        private static void MemberRemovedRolesHandler(
            IMemberService sender,
            RolesEventArgs e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMemberRemovedRolesEventService>();

            foreach (var service in services)
            {
                service.MemberRemovedRolesHandler(sender, e);
            }
        }

        private static void AssignedRolesHandler(
            IMemberService sender,
            RolesEventArgs e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMemberAssignedRolesEventService>();

            foreach (var service in services)
            {
                service.MemberAssignedRolesHandler(sender, e);
            }
        }

        private static void MemberGroupSavedHandler(
            IMemberGroupService sender,
            SaveEventArgs<IMemberGroup> e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMemberGroupSavedEventService>();

            foreach (var service in services)
            {
                service.MemberGroupSavedHandler(sender, e);
            }
        }

        private static void MemberGroupDeletingHandler(
            IMemberGroupService sender,
            DeleteEventArgs<IMemberGroup> e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMemberGroupDeletingEventService>();

            foreach (var service in services)
            {
                service.MemberGroupDeleteHandler(sender, e);
            }
        }

        private static void ProcessMediaSaving(
            IMediaService sender,
            SaveEventArgs<IMedia> e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMediaSavingEventService>();

            foreach (var service in services)
            {
                service.ProcessMediaSaving(sender, e);
            }
        }

        private static void ProcessMediaTrashed(
            IMediaService sender,
            MoveEventArgs<IMedia> e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMediaTrashedEventService>();

            foreach (var service in services)
            {
                service.ProcessMediaTrashed(sender, e);
            }
        }

        private static void ProcessMediaSaved(
            IMediaService sender,
            SaveEventArgs<IMedia> e)
        {
            var services =
                DependencyResolver.Current.GetServices<IUmbracoMediaSavedEventService>();

            foreach (var service in services)
            {
                service.ProcessMediaSaved(sender, e);
            }
        }

        private static void MemberDeletingHandler(IMemberService sender, DeleteEventArgs<IMember> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberDeletingEventService>();
            foreach (var service in services) service.MemberDeleteHandler(sender, e);
        }
        
        private static void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentTrashedEventService>();
            foreach (var service in services) service.ProcessContentTrashed(sender, e);
        }
        private static void ProcessContentPublished(IContentService sender, ContentPublishedEventArgs e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentPublishedEventService>();
            foreach (var service in services) service.ProcessContentPublished(sender, e);
        }

        private static void ProcessContentUnPublished(IContentService sender, PublishEventArgs<IContent>  e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentUnPublishedEventService>();
            foreach (var service in services) service.ProcessContentUnPublished(sender, e);
        }
    }
}