using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uIntra.Users
{
    public class IntranetUserService : IIntranetUserService<IntranetUser>
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string UmbracoUserIdPropertyAlias => "relatedUser";

        protected virtual string IntranetUsersCacheKey => "IntranetUsersCache";
        protected virtual string CurrentUserCacheKey => "CurrentUserCache";

        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IRoleService _roleService;
        private readonly IIntranetRoleTypeProvider _intranetRoleTypeProvider;
        private readonly ICacheService _cache;

        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cache)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
            _roleService = roleService;
            _intranetRoleTypeProvider = intranetRoleTypeProvider;
            _cache = cache;
        }

        public virtual IntranetUser Get(int umbracoId)
        {
            var member = GetAll().SingleOrDefault(el => el.UmbracoId == umbracoId);
            return member;
        }

        public virtual IntranetUser Get(Guid id)
        {
            var member = GetAll().SingleOrDefault(el => el.Id == id);
            return member;
        }

        public virtual IntranetUser Get(IHaveCreator model)
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
            return (IntranetUser)member;
        }

        public virtual IEnumerable<IntranetUser> GetMany(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(GetAll(),
               id => id,
               user => user.Id,
               (id, user) => user);
        }

        public virtual IEnumerable<IntranetUser> GetMany(IEnumerable<int> ids)
        {
            return ids.Distinct().Join(GetAll(),
                 id => id,
                 user => user.UmbracoId.GetValueOrDefault(),
                 (id, user) => user);
        }

        public virtual IEnumerable<IntranetUser> GetAll()
        {
            var users = _cache.GetOrSet(IntranetUsersCacheKey, GetAllFromSql, CacheHelper.GetMidnightUtcDateTimeOffset()).ToList();
            return users;
        }

        public virtual IntranetUser GetCurrentUser()
        {
            var currentUser = _cache.GetOrSet(CurrentUserCacheKey, GetCurrentUserFromSql, CacheHelper.GetMidnightUtcDateTimeOffset());
            return currentUser;
        }

        public virtual IEnumerable<IntranetUser> GetByRole(int role)
        {
            var users = GetAll().Where(el => el.Role.Priority == role);
            return users;
        }

        protected virtual IEnumerable<IntranetUser> GetAllFromSql()
        {
            var members = _memberService.GetAllMembers().Select(Map).ToList();
            return members;
        }

        protected virtual IntranetUser GetCurrentUserFromSql()
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

        protected virtual IntranetUser Map(IMember member)
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

            string userPhoto = null;
            var userPhotoId = member.GetValueOrDefault<int?>("photo");

            if (userPhotoId.HasValue)
            {
                userPhoto = _umbracoHelper.TypedMedia(userPhotoId.Value)?.Url;
            }

            user.Photo = GetUserPhotoOrDefaultAvatar(userPhoto);

            return user;
        }

        protected virtual IRole GetMemberRole(IMember member)
        {
            var roles = _memberService.GetAllRoles(member.Id).ToList();
            return _roleService.GetActualRole(roles);
        }

        protected virtual string GetGroupNameFromRole(int role)
        {
            var roleMode = _intranetRoleTypeProvider.Get(role);
            return roleMode.Name;
        }

        protected virtual string GetUserPhotoOrDefaultAvatar(string userImage)
        {
            return !string.IsNullOrEmpty(userImage) ? userImage : string.Empty;
        }

        protected virtual IntranetUser GetByName(string name)
        {
            var user = _memberService.GetByUsername(name);

            if (user == null)
            {
                return null;
            }

            return Map(user);
        }
    }
}
