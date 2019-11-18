﻿using System;
using System.Threading.Tasks;
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

        public static async Task<Guid> GetCurrentMemberIdAsync(this IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            var currentMember = await intranetMemberService.GetCurrentMemberAsync();
            return currentMember?.Id ?? Guid.Empty;
        }
    }
}