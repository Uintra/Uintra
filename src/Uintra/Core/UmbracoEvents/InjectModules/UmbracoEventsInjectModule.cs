using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Article.Events;
using Uintra.Core.Search;
using Uintra.Core.UmbracoEvents.Services.Contracts;
using Uintra.Core.UmbracoEvents.Services.Implementations;
using Uintra.Features.Tagging.UserTags;

namespace Uintra.Core.UmbracoEvents.InjectModules
{
    public class UmbracoEventsInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            // member
            services.AddScoped<IUmbracoMemberCreatedEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberAssignedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberRemovedRolesEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberDeletingEventService, MemberEventService>();
            services.AddScoped<IUmbracoMemberGroupDeletingEventService, MemberGroupEventService>();
            services.AddScoped<IUmbracoMemberGroupSavedEventService, MemberGroupEventService>();

            // media
            services.AddScoped<IUmbracoMediaSavedEventService, VideoConvertEventService>();
            //services.AddScoped<IUmbracoMediaTrashedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavedEventService, SearchMediaEventService>();
            //services.AddScoped<IUmbracoMediaSavingEventService, SearchMediaEventService>();
            
            //content
            services.AddScopedToCollection<IUmbracoContentTrashedEventService, ArticlePageEventService>();
            services.AddScopedToCollection<IUmbracoContentPublishedEventService, ArticlePageEventService>();
            services.AddScopedToCollection<IUmbracoContentUnPublishedEventService, ArticlePageEventService>();
            
            //user tags
            services.AddScopedToCollection<IUmbracoContentTrashedEventService, UserTagsEventService>();
            services.AddScopedToCollection<IUmbracoContentPublishedEventService, UserTagsEventService>();
            services.AddScopedToCollection<IUmbracoContentUnPublishedEventService, UserTagsEventService>();

            return services;
        }
    }
}
