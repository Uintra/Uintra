using System;
using uIntra.Core.User;

namespace uIntra.Core
{
    public static class IntranetUserServiceExtensions
    {
        public static Guid GetCurrentUserId(this IIntranetUserService<IIntranetUser> intranetUserService)
        {
            var currentUser = intranetUserService.GetCurrentUser();
            return currentUser.Id;
        }

        public static int? GetCurrentUserUmbracoId(this IIntranetUserService<IIntranetUser> intranetUserService)
        {
            var currentUser = intranetUserService.GetCurrentUser();
            return currentUser.UmbracoId;
        }
    }
}