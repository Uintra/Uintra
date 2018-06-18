using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.uIntra.Controllers.Api;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace uIntra.Users.Web
{
    public abstract class UserApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IMemberServiceHelper _memberServiceHelper;

        protected UserApiControllerBase(IUserService userService, IMemberService memberService, IMemberServiceHelper memberServiceHelper)
        {
            _userService = userService;
            _memberService = memberService;
            _memberServiceHelper = memberServiceHelper;
        }


        [HttpGet]
        public virtual IEnumerable<UserPickerModel> NotAssignedToMemberUsers()
        {
            int totalUsersCount;
            var users = _userService.GetAll(0, int.MaxValue, out totalUsersCount);
            var relatedUserIdsDictionary = _memberServiceHelper.GetRelatedUserIdsForMembers(_memberService.GetAllMembers());
            var unassignedUsers = users.Where(u => !relatedUserIdsDictionary.Values.Contains(u.Id));
            var mappedModels = unassignedUsers.Select(u => new UserPickerModel {Id = u.Id, Name = u.Name});
            return mappedModels;
        }
    }
}