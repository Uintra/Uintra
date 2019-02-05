using System;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Extensions;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace Uintra.Core.User
{
    public class IntranetUserService<T> : IIntranetUserService<T> where T : class, IIntranetUser, new()
    {
        private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IApplicationSettings _applicationSettings;

        public IntranetUserService(IMemberService memberService, IUserService userService, IApplicationSettings applicationSettings)
        {
            _memberService = memberService;
            _userService = userService;
            _applicationSettings = applicationSettings;
        }

        public T AddUser(IIntranetMember member)
        {
            var umbracoUser = GetUser(member.Email);

            if (umbracoUser == null)
            {

            }

            return null;

        }

        public T GetUser(string email)
        {
            var umbracoUser = _userService.GetByEmail(email);
            return umbracoUser == null ? null : Map(umbracoUser);
        }

        public T GetUser(int id)
        {
            var umbracoUser = _userService.GetUserById(id);
            return umbracoUser == null ? null : Map(umbracoUser);
        }

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

        //protected virtual IIntranetUser CreateUser(IIntranetMember member)
        //{
        //    var umbracoMember = _memberService.GetByEmail(member.Email);

        //    _userService.CreateWithIdentity(member.DisplayedName, member.Email, umbracoMember.RawPasswordValue, userTypeAlias);
        //}
    }
}
