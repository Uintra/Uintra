using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uintra.Core;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using Umbraco.Core;

namespace Uintra.Users.Web
{
    public abstract class UserApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IMemberServiceHelper _memberServiceHelper;

        protected UserApiControllerBase(
            IUserService userService,
            IMemberService memberService,
            IMemberServiceHelper memberServiceHelper)
        {
            _userService = userService;
            _memberService = memberService;
            _memberServiceHelper = memberServiceHelper;
        }

        [HttpGet]
        public virtual IEnumerable<UserPickerModel> NotAssignedToMemberUsers(int? selectedUserId)
        {
            var users = _userService.GetAll(0, int.MaxValue, out _);

            var relatedUserIdsDictionary = _memberServiceHelper.GetRelatedUserIdsForMembers(_memberService.GetAllMembers());
            var unassignedUsers = users.Where(u => !relatedUserIdsDictionary.Values.Contains(u.Id) || u.Id == selectedUserId);
            var mappedModels = unassignedUsers.Select(u => new UserPickerModel {Id = u.Id, Name = u.Name});

            return mappedModels;
        }

        [HttpGet]
        public virtual IEnumerable<ProfileColumnModel> ProfileProperties()
        {
            return ReflectionHelper.GetProfileColumns();
        }
    }
}