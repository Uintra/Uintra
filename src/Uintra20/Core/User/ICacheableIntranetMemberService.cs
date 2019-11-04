using System;
using System.Collections.Generic;

namespace Uintra20.Core.User
{
    public interface ICacheableIntranetMemberService
    {
        void UpdateMemberCache(Guid memberId);

        void UpdateMemberCache(int memberId);

        void UpdateMemberCache(IEnumerable<Guid> memberIds);

        void DeleteFromCache(Guid memberId);
    }
}
