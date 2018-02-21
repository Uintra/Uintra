using System;
using Uintra.Core.User;

namespace Uintra.Core.Extensions
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

        public static Guid GetCurrentBackOfficeUserId(this IIntranetUserService<IIntranetUser> intranetUserService)
        {
            return intranetUserService.GetCurrentBackOfficeUser().Id;
        }
    }
}