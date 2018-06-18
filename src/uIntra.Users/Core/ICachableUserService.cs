using System;
using System.Collections.Generic;

namespace Uintra.Users
{
    public interface ICacheableIntranetUserService
    {
        void UpdateUserCache(Guid userId);

        void UpdateUserCache(IEnumerable<Guid> userIds);

        void DeleteFromCache(Guid userId);
    }
}