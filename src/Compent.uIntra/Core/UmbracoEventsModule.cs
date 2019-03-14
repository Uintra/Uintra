using System.Web.Mvc;
using Uintra.Core.Core.UmbracoEventServices;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core
{
    public static class UmbracoEventsModule
    {
        public static void RegisterEvents()
        {
            ContentService.Published += ProcessContentPublished;
            ContentService.UnPublished += ProcessContentUnPublished;
            ContentService.Trashed += ProcessContentTrashed;
            ContentService.Saving += ProcessPanelSaving;

            MemberService.AssignedRoles += ProcessMemberAssignedRoles;
            MemberService.RemovedRoles += ProcessMemberRemovedRoles;
            MemberService.Deleting += ProcessMemberDeleting;
            MemberService.Created += ProcessMemberCreated;
            MediaService.Saved += ProcessMediaSaved;
            MediaService.Trashed += ProcessMediaTrashed;
            MediaService.Saving += ProcessMediaSaving;

            MemberGroupService.Deleting += ProcessMemberGroupDeliting;
            MemberGroupService.Saved += ProcessMemberGroupSaved;
        }

        private static void ProcessMemberRemovedRoles(IMemberService sender, RolesEventArgs e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberRemovedRolesEventService>();
            foreach (var service in services) service.ProcessMemberRemovedRoles(sender, e);
        }

        private static void ProcessMemberAssignedRoles(IMemberService sender, RolesEventArgs e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberAssignedRolesEventService>();
            foreach (var service in services) service.ProcessMemberAssignedRoles(sender, e);
        }

        private static void ProcessMemberGroupSaved(IMemberGroupService sender, SaveEventArgs<IMemberGroup> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberGroupSavedEventService>();
            foreach (var service in services) service.ProcessMemberGroupSaved(sender, e);
        }

        private static void ProcessMemberCreated(IMemberService sender, NewEventArgs<IMember> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberCreatedEventService>();
            foreach (var service in services) service.ProcessMemberCreated(sender, e);
        }

        private static void ProcessPanelSaving(IContentService sender, SaveEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentSavingEventService>();
            foreach (var service in services) service.ProcessContentSaving(sender, e);
        }

        private static void ProcessMemberGroupDeliting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberGroupDeletingEventService>();
            foreach (var service in services) service.ProcessMemberGroupDeleting(sender, e);
        }

        private static void ProcessMediaSaving(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMediaSavingEventService>();
            foreach (var service in services) service.ProcessMediaSaving(sender, e);
        }

        private static void ProcessMediaTrashed(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMediaTrashedEventService>();
            foreach (var service in services) service.ProcessMediaTrashed(sender, e);
        }

        private static void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMediaSavedEventService>();
            foreach (var service in services) service.ProcessMediaSaved(sender, e);
        }

        private static void ProcessMemberDeleting(IMemberService sender, DeleteEventArgs<IMember> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoMemberDeletingEventService>();
            foreach (var service in services) service.ProcessMemberDeleting(sender, e);
        }

        private static void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentPublishedEventService>();
            foreach (var service in services) service.ProcessContentPublished(sender, e);
        }

        private static void ProcessContentUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentUnPublishedEventService>();
            foreach (var service in services) service.ProcessContentUnPublished(sender, e);
        }

        private static void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            var services = DependencyResolver.Current.GetServices<IUmbracoContentTrashedEventService>();
            foreach (var service in services) service.ProcessContentTrashed(sender, e);
        }
    }
}