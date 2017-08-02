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
    public class IntranetUserService : IIntranetUserService<IntranetUser>, ICacheableIntranetUserService
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string IntranetUsersCacheKey => "IntranetUsersCache";

        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IRoleService _roleService;
        private readonly IIntranetRoleTypeProvider _intranetRoleTypeProvider;
        private readonly ICacheService _cacheService;

        public IntranetUserService(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            IIntranetRoleTypeProvider intranetRoleTypeProvider,
            ICacheService cacheService)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
            _roleService = roleService;
            _intranetRoleTypeProvider = intranetRoleTypeProvider;
            _cacheService = cacheService;
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
            var users = _cacheService.GetOrSet(IntranetUsersCacheKey, GetAllFromSql, CacheHelper.GetMidnightUtcDateTimeOffset()).ToList();
            return users;
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

        public virtual IEnumerable<IntranetUser> GetByRole(int role)
        {
            var users = GetAll().Where(el => el.Role.Priority == role);
            return users;
        }

        public virtual void Save(IntranetUserDTO user)
        {
            var member = _memberService.GetByKey(user.Id);
            member.SetValue(ProfileConstants.FirstName, user.FirstName);
            member.SetValue(ProfileConstants.LastName, user.LastName);

            if (user.NewMedia.HasValue)
            {
                member.SetValue(ProfileConstants.Photo, user.NewMedia.Value);
            }

            if (user.DeleteMedia)
            {
                member.SetValue(ProfileConstants.Photo, null);
            }

            _memberService.Save(member);

            UpdateUserCache(user.Id);
        }

        protected virtual IntranetUser GetFromSql(Guid id)
        {
            var member = _memberService.GetByKey(id);
            return Map(member);
        }

        protected virtual IEnumerable<IntranetUser> GetAllFromSql()
        {
            var members = _memberService.GetAllMembers().Select(Map).ToList();
            return members;
        }

        protected virtual IntranetUser Map(IMember member)
        {
            var user = new IntranetUser
            {
                Id = member.Key,
                UmbracoId = member.GetValueOrDefault<int?>(ProfileConstants.RelatedUser),
                Email = member.Email,
                FirstName = member.GetValueOrDefault<string>(ProfileConstants.FirstName),
                LastName = member.GetValueOrDefault<string>(ProfileConstants.LastName),
                LoginName = member.Username,
                Role = GetMemberRole(member)
            };

            string userPhoto = null;
            var userPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo);

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
            var users = GetAll();
            return users.FirstOrDefault(user => user.LoginName.Equals(name));
        }

        public virtual void UpdateUserCache(Guid userId)
        {
            var updatedUser = GetFromSql(userId);

            var allCachedUsers = GetAll().ToList();
            var oldCachedUser = allCachedUsers.Find(el => el.Id == userId);

            if (oldCachedUser != null)
            {
                allCachedUsers.Remove(oldCachedUser);
            }

            if (updatedUser != null)
            {
                allCachedUsers.Add(updatedUser);
            }

            _cacheService.Set(IntranetUsersCacheKey, allCachedUsers, CacheHelper.GetMidnightUtcDateTimeOffset());
        }
    }
}
