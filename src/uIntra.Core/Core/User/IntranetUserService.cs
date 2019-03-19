using System;
using LanguageExt;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Extensions;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Uintra.Core.User
{
    public class IntranetUserService<T> : IIntranetUserService<T> where T : class, IIntranetUser, new()
    {
        private readonly IUserService _userService;
        private readonly IApplicationSettings _applicationSettings;

        public IntranetUserService(IUserService userService, IApplicationSettings applicationSettings)
        {
            _userService = userService;
            _applicationSettings = applicationSettings;
        }

        public Option<T> GetByEmailOrNone(string email) =>
            Optional(_userService.GetByEmail(email)).Map(Map);

        public Option<T> GetByIdOrNone(int id) =>
            Optional(_userService.GetUserById(id)).Map(Map);

        protected virtual T Map(IUser umbracoUser)
        {
            var user = new T
            {
                IsSuperUser = _applicationSettings.UintraSuperUsers.Contains(umbracoUser.Email, StringComparison.InvariantCultureIgnoreCase),
                DisplayName = umbracoUser.Name,
                Email = umbracoUser.Email,
                Id = umbracoUser.Id,
                IsApproved = umbracoUser.IsApproved,
                IsLockedOut = umbracoUser.IsLockedOut
            };

            return user;

        }
    }
}
