using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Helpers;
using Uintra.Core.Member.Models;
using Uintra.Core.Member.Services;

namespace Uintra.Infrastructure.Extensions
{
    public static class IntranetMemberServiceExtensions
    {
        public static Guid GetCurrentMemberId(this IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            var currentMember = intranetMemberService.GetCurrentMember();
            return currentMember?.Id ?? Guid.Empty;
        }
        public static async Task<Guid> GetCurrentMemberIdAsync(this IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            var currentMember = await intranetMemberService.GetCurrentMemberAsync();
            return currentMember?.Id ?? Guid.Empty;
        }

        public static MemberViewModel ToViewModel(this IIntranetMember member)
        {
            return DependencyResolver.Current.GetService<IMemberServiceHelper>().ToViewModel(member);
        }
    }
}