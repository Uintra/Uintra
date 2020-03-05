using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Member.Events;
using Uintra20.Core.UmbracoEventServices;

namespace Uintra20.Infrastructure.Ioc
{
    public class UmbracoEventsInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IUmbracoMemberCreatedEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberAssignedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberRemovedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberDeletingEventService, MemberEventService>();
            //services.AddScoped<IUmbracoContentPublishedEventService, SearchContentEventService>();
            //services.AddScoped<IUmbracoContentUnPublishedEventService, SearchContentEventService>();
            //services.AddScoped<IUmbracoMediaTrashedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavingEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoContentTrashedEventService, DeleteUserTagHandler>();
            //services.AddScoped<IUmbracoContentPublishedEventService, ContentPageRelationHandler>();
            //services.AddScoped<IUmbracoContentTrashedEventService, ContentPageRelationHandler>();
            //services.AddScoped<IUmbracoContentPublishedEventService, CreateUserTagHandler>();
            //services.AddScoped<IUmbracoContentUnPublishedEventService, CreateUserTagHandler>();
            //services.AddScoped<IUmbracoMediaSavedEventService, VideoConvertEventService>();
            //services.AddScoped<IUmbracoMemberGroupDeletingEventService, UmbracoMemberGroupEventService>();
            //services.AddScoped<IUmbracoMemberGroupSavedEventService, UmbracoMemberGroupEventService>();
            //services.AddScoped<IUmbracoContentSavingEventService, UmbracoContentSavingEventService>();

            return services;
        }
    }
}
