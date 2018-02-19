using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using CacheHelper = Uintra.Core.Caching.CacheHelper;

namespace Uintra.Users
{
    public abstract class IntranetUserServiceBase<T> : IIntranetUserService<T>, ICacheableIntranetUserService
          where T : IIntranetUser, new()
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string UsersCacheKey => "IntranetUsersCache";

        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IRoleService _roleService;
        private readonly ICacheService _cacheService;

        protected IntranetUserServiceBase(
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            ICacheService cacheService)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
            _roleService = roleService;
            _cacheService = cacheService;
        }

        public virtual T Get(IHaveOwner model) => Get(model.OwnerId);

        public virtual T Get(Guid id) => GetSingle(el => el.Id == id);

        public virtual T Get(int umbracoId)
        {
            return GetAll().FirstOrDefault(m => m.UmbracoId == umbracoId);
        }

        private T GetSingle(Func<T, bool> predicate)
        {
            var member = GetAll().SingleOrDefault(predicate);
            return member;
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(GetAll(),
               id => id,
               user => user.Id,
               (id, user) => user);
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<int> ids)
        {
            return ids.Distinct().Join(GetAll(),
                 id => id,
                 user => user.UmbracoId.GetValueOrDefault(),
                 (id, user) => user);
        }

        public virtual IEnumerable<T> GetAll()
        {
            var users = _cacheService.GetOrSet(UsersCacheKey, GetAllFromSql, CacheHelper.GetMidnightUtcDateTimeOffset()).ToList();
            return users;
        }

        public virtual T GetCurrentUser()
        {
            string userName = GetCurrentUserName();
            var user = GetByName(userName);
            return user;
        }

        protected virtual string GetCurrentUserName()
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

            return userName;
        }

        public virtual IEnumerable<T> GetByRole(int role)
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

            _memberService.Save(member, raiseEvents: false);

            UpdateUserCache(user.Id);
        }

        protected virtual T GetFromSql(Guid id)
        {
            var member = _memberService.GetByKey(id);
            return member != null ? Map(member) : default(T);
        }

        protected virtual IEnumerable<T> GetAllFromSql()
        {
            var members = _memberService.GetAllMembers().Select(Map).ToList();
            return members;
        }

        protected virtual T Map(IMember member)
        {
            var user = new T
            {
                Id = member.Key,
                UmbracoId = member.GetValueOrDefault<int?>(ProfileConstants.RelatedUser),
                Email = member.Email,
                LoginName = member.Username,
                Role = GetMemberRole(member),
                Inactive = member.IsLockedOut
            };

            string userPhoto = null;
            var userPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo) ?? member.GetMemberImageId(ProfileConstants.Photo);

            if (userPhotoId.HasValue)
            {
                userPhoto = _umbracoHelper.TypedMedia(userPhotoId.Value)?.Url;
            }

            user.Photo = GetUserPhotoOrDefaultAvatar(userPhoto);

            return user;
        }

        protected virtual IEnumerable<T> GetUnassignedToMemberUsers()
        {
            var users = GetAll();
            var assignedUsersIds = _memberService.GetAllMembers().Select(m => m.GetValue<Guid>("relatedUser"));
            var unassignedUsers = users.Join(assignedUsersIds, user => user.Id, id => id, (user, id) => user);

            return unassignedUsers;
        }

        protected virtual IRole GetMemberRole(IMember member)
        {
            var roles = _memberService.GetAllRoles(member.Id).ToList();
            return _roleService.GetActualRole(roles);
        }

        protected virtual string GetUserPhotoOrDefaultAvatar(string userImage)
        {
            return !string.IsNullOrEmpty(userImage) ? userImage : string.Empty;
        }

        public virtual T GetByName(string name)
        {
            var users = GetAll();
            return users.SingleOrDefault(user => user.LoginName.ToLowerInvariant().Equals(name.ToLowerInvariant()));
        }

        public virtual T GetByEmail(string email)
        {
            var users = GetAll();
            return users.SingleOrDefault(user => user.Email.ToLowerInvariant().Equals(email.ToLowerInvariant()));
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

            _cacheService.Set(UsersCacheKey, allCachedUsers, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual void DeleteFromCache(Guid userId)
        {
            var allCachedUsers = GetAll().ToList();
            var oldCachedUser = allCachedUsers.Find(el => el.Id == userId);

            if (oldCachedUser != null)
            {
                allCachedUsers.Remove(oldCachedUser);
            }
            _cacheService.Set(UsersCacheKey, allCachedUsers, CacheHelper.GetMidnightUtcDateTimeOffset());            
        }
    }
}
