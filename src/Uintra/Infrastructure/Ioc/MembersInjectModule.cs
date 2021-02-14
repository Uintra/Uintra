using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Helpers;
using Uintra.Core.Member.Profile.Services;
using Uintra.Core.Member.Services;
using Uintra.Core.User;
using Uintra.Core.User.Models;
using Uintra.Features.Tagging.UserTags.Services;

namespace Uintra.Infrastructure.Ioc
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