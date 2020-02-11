using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Groups.ContentServices;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Services;

namespace Uintra20.Infrastructure.Ioc
{
    public class GroupInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupHelper, GroupHelper>();
            services.AddScoped<IGroupContentProvider, GroupContentProvider>();
            services.AddScoped<IGroupMemberService, GroupMemberService>();
            services.AddScoped<IGroupMediaService, GroupMediaService>();
            services.AddScoped<IGroupLinkProvider, GroupLinkProvider>();
            services.AddScoped<IGroupDocumentsService, GroupDocumentsService>();

            return services;
        }
    }
}