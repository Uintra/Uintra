using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Groups.ContentServices;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Links;
using Uintra.Features.Groups.Services;

namespace Uintra.Infrastructure.Ioc
{
    public class GroupInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupContentProvider, GroupContentProvider>();
            services.AddScoped<IGroupMemberService, GroupMemberService>();
            services.AddScoped<IGroupMediaService, GroupMediaService>();
            services.AddScoped<IGroupLinkProvider, GroupLinkProvider>();
            services.AddScoped<IGroupDocumentsService, GroupDocumentsService>();
            services.AddScoped<IGroupHelper, GroupHelper>();

            return services;
        }
    }
}