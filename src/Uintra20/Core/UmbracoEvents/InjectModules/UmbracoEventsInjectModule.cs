using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Core.UmbracoEvents.Services.Implementations;

namespace Uintra20.Core.UmbracoEvents.InjectModules
{
    public class UmbracoEventsInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IUmbracoMemberCreatedEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberAssignedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberRemovedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberDeletingEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberGroupDeletingEventService, MemberGroupEventService>();
            services.AddScoped<IUmbracoMemberGroupSavedEventService, MemberGroupEventService>();
            
            //services.AddScoped<IUmbracoMediaTrashedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavingEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavedEventService, VideoConvertEventService>();

            //services.AddScoped<IUmbracoContentTrashedEventService, DeleteUserTagHandler>();
            //services.AddScoped<IUmbracoContentPublishedEventService, ContentPageRelationHandler>();
            //services.AddScoped<IUmbracoContentTrashedEventService, ContentPageRelationHandler>();
            //services.AddScoped<IUmbracoContentPublishedEventService, CreateUserTagHandler>();
            //services.AddScoped<IUmbracoContentUnPublishedEventService, CreateUserTagHandler>();
            //services.AddScoped<IUmbracoContentSavingEventService, UmbracoContentSavingEventService>();
            //services.AddScoped<IUmbracoContentPublishedEventService, SearchContentEventService>();
            //services.AddScoped<IUmbracoContentUnPublishedEventService, SearchContentEventService>();

            return services;
        }
    }
}
