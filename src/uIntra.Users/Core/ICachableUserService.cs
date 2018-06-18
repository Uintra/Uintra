using System;

namespace Uintra.Users
{
    public interface ICacheableIntranetUserService
    {
        void UpdateUserCache(Guid userId);

        void DeleteFromCache(Guid userId);
    }
}