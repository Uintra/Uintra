using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Exceptions;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uIntra.Users.Core
{
    public class IntranetUserService : IIntranetUserService<IntranetUser>
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string UmbracoUserIdPropertyAlias => "relatedUser";
        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IApplicationSettings _applicationSettings;

        public IntranetUserService(IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IExceptionLogger exceptionLogger,
            IApplicationSettings applicationSettings)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
            _exceptionLogger = exceptionLogger;
            _applicationSettings = applicationSettings;
        }

        public virtual IntranetUser Get(int umbracoId)
        {
            var member = _memberService.GetMembersByPropertyValue(UmbracoUserIdPropertyAlias, umbracoId).SingleOrDefault();

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public virtual IntranetUser Get(Guid id)
        {
            var member = _memberService.GetByKey(id);

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public virtual IEnumerable<IntranetUser> GetAll()
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members;
        }

        public virtual IEnumerable<IntranetUser> GetMany(IEnumerable<Guid> ids)
        {
            var members = _memberService.GetAllMembers().Where(s => ids.Contains(s.Key)).Select(Map);
            return members;
        }

        public virtual IEnumerable<IntranetUser> GetMany(IEnumerable<int> ids)
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members.Where(s => s.UmbracoId.HasValue && ids.Contains(s.UmbracoId.Value));
        }

        public virtual void FillCreator(IHaveCreator model)
        {
            IIntranetUser member;

            if (model.UmbracoCreatorId.HasValue)
            {
                member = Get(model.UmbracoCreatorId.Value);
            }
            else
            {
                member = Get(model.CreatorId);
            }
            model.Creator = member;
        }

        public virtual IntranetUser GetCurrentUser()
        {
            var userName = "";
            if (HostingEnvironment.IsHosted) //TODO: WTF IS THIS
            {
                var httpContext = _umbracoContext.HttpContext;
                if (httpContext.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    userName = httpContext.User.Identity.Name;
                }
            }
            if (string.IsNullOrEmpty(userName))
            {
                var currentPrincipal = Thread.CurrentPrincipal;
                if (currentPrincipal?.Identity != null)
                {
                    userName = currentPrincipal.Identity.Name;
                }
            }
            var user = GetByName(userName);
            return user;
        }

        public virtual IntranetUser Map(IMember member)
        {
            var user = new IntranetUser
            {
                Id = member.Key,
                UmbracoId = member.GetValueOrDefault<int?>(UmbracoUserIdPropertyAlias),
                Email = member.Email,
                FirstName = member.GetValueOrDefault<string>("firstName"),
                LastName = member.GetValueOrDefault<string>("lastName"),
                Role = GetMemberRole(member)
            };

            var userPhotoId = member.GetValueOrDefault<int?>("photo");
            if (userPhotoId.HasValue)
            {
                var media = _umbracoHelper.TypedMedia(userPhotoId.Value);
                user.Photo = GetUserPhotoOrDefaultAvatar(media.Url);
            }
            return user;
        }

        public virtual IEnumerable<IntranetUser> GetByRole(IntranetRolesEnum role)
        {
            var members = _memberService.GetMembersByGroup(GetGroupNameFromRole(role));
            return members.Select(Map);
        }

        private IntranetUser GetByName(string name)
        {
            var user = _memberService.GetByUsername(name);

            if (user == null)
            {
                return null;
            }

            return Map(user);
        }

        protected virtual IntranetRolesEnum GetMemberRole(IMember member)
        {
            var roles = _memberService.GetAllRoles(member.Id).Select(GetRoleFromGroupName).ToList();

            if (roles.Count != 0)
            {
                if (roles.Count > 1)
                {
                    _exceptionLogger.Log(new Exception($"Member \"{member.Name}\" - \"{member.Id}\" has more then one role!"));
                }

                var highestRole = roles.Min();
                return highestRole;
            }

            _exceptionLogger.Log(new Exception($"Member \"{member.Name}\" - \"{member.Id}\" has no role!"));

            return IntranetRolesEnum.UiUser;
        }

        protected virtual IntranetRolesEnum GetRoleFromGroupName(string groupName)
        {
            IntranetRolesEnum role;
            if (Enum.TryParse(groupName, out role))
            {
                return role;
            }

            throw new Exception($"Can't map group name {groupName} to IntranetUserRole");
        }

        protected virtual string GetGroupNameFromRole(IntranetRolesEnum role)
        {
            return role.ToString();
        }

        protected virtual string GetUserPhotoOrDefaultAvatar(string userImage)
        {
            return !string.IsNullOrEmpty(userImage) ? userImage : _applicationSettings.DefaultAvatarPath;
        }
    }
}
