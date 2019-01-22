using System;
using System.Collections.Generic;

namespace Uintra.Users
{
    public interface ICacheableIntranetMemberService
    {
        void UpdateMemberCache(Guid memberId);

        void UpdateMemberCache(IEnumerable<Guid> memberIds);

        void DeleteFromCache(Guid memberId);
    }
}