using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Profile.Services;
using Uintra20.Core.Member.Services;
using Uintra20.Core.User;
using Uintra20.Core.User.Models;
using Uintra20.Features.Groups;
using Uintra20.Features.Tagging.UserTags.Services;

namespace Uintra20.Infrastructure.Ioc
{
	public class MembersInjectModule : IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			services.AddScoped<IIntranetMemberService<IntranetMember>, IntranetMemberService<IntranetMember>>();
            services.AddScoped<ICacheableIntranetMemberService, IntranetMemberService<IntranetMember>>();
            services.AddScoped<IIntranetUserService<IntranetUser>, IntranetUserService<IntranetUser>>();
            services.AddScoped<IIntranetUserContentProvider, IntranetUserContentProvider>();
            services.AddScoped<IUserTagProvider, UserTagProvider>();
            services.AddScoped<IUserTagRelationService, UserTagRelationService>();
            services.AddScoped<IUserTagService, UserTagService>();
            services.AddScoped<IMemberServiceHelper, MemberServiceHelper>();
			services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IMentionService, MentionService>();

            return services;
		}
	}
}