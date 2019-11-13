using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.User
{
    public interface ICacheableIntranetMemberService
    {
        void UpdateMemberCache(Guid memberId);
        void UpdateMemberCache(int memberId);
        void UpdateMemberCache(IEnumerable<Guid> memberIds);
        void DeleteFromCache(Guid memberId);

        Task UpdateMemberCacheAsync(Guid memberId);
        Task UpdateMemberCacheAsync(int memberId);
        Task UpdateMemberCacheAsync(IEnumerable<Guid> memberIds);
        Task DeleteFromCacheAsync(Guid memberId);
    }
}
