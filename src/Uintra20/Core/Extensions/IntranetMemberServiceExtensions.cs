using System;
using Uintra20.Core.User;

namespace Uintra20.Core.Extensions
{
    public static class IntranetMemberServiceExtensions
    {
        public static Guid GetCurrentMemberId(this IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            var currentMember = intranetMemberService.GetCurrentMember();
            return currentMember?.Id ?? Guid.Empty;
        }
    }
}