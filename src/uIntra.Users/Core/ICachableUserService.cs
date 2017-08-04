using System;

namespace uIntra.Users
{
    public interface ICacheableIntranetUserService
    {
        void UpdateUserCache(Guid userId);
    }
}