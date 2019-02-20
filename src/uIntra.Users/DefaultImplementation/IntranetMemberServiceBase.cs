using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.User;
using Uintra.Core.User.DTO;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using static LanguageExt.Prelude;
using CacheHelper = Uintra.Core.Caching.CacheHelper;

namespace Uintra.Users
{
    public abstract class IntranetMemberServiceBase<T> : IIntranetMemberService<T>, ICacheableIntranetMemberService
          where T : IIntranetMember, new()
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string MembersCacheKey => "IntranetMembersCache";

        private readonly IMediaService _mediaService;
        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IRoleService _roleService;
        private readonly ICacheService _cacheService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        protected IntranetMemberServiceBase(
            IMediaService mediaService,
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            IRoleService roleService,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _mediaService = mediaService;
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
            _roleService = roleService;
            _cacheService = cacheService;
            _intranetUserService = intranetUserService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }

        public virtual bool IsCurrentMemberSuperUser
        {
            get
            {
                var currentMember = GetCurrentMember();
                return currentMember != null && currentMember.IsSuperUser;
            }
        }

        public virtual T Get(IHaveOwner model) => Get(model.OwnerId);

        public virtual T Get(Guid id) => GetSingle(el => el.Id == id);

        public virtual T Get(int umbracoId)
        {
            return GetSingle(el => el.RelatedUser?.Id == umbracoId);
        }

        private T GetSingle(Func<T, bool> predicate)
        {
            var member = GetAll().Single(predicate);
            return member;
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(GetAll(),
               id => id,
               member => member.Id,
               (id, member) => member);
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<int> ids)
        {
            return ids.Distinct().Join(GetAll(),
                 id => id,
                member => member.RelatedUser?.Id,
                 (id, member) => member);
        }

        public virtual IEnumerable<T> GetAll()
        {
            var members = _cacheService.GetOrSet(MembersCacheKey, GetAllFromSql, CacheHelper.GetMidnightUtcDateTimeOffset()).ToList();
            return members;
        }

        public virtual T GetCurrentMember()
        {
            var member = _umbracoHelper.MembershipHelper.GetCurrentMember();
            if (member != null) return Get(member.GetKey());

            var umbracoUser = _umbracoContext.Security.CurrentUser;
            if (umbracoUser != null) return Get(umbracoUser.Id);

            return default;
        }

        public virtual IEnumerable<T> GetByGroup(int role)
        {
            var members = GetAll().Where(el => el.Group.Id == role);
            return members;
        }

        public virtual bool Update(UpdateMemberDto dto)
        {
            var member = _memberService.GetByKey(dto.Id);
            var isPresent = member != null;
            if (isPresent)
            {
                member.SetValue(ProfileConstants.FirstName, dto.FirstName);
                member.SetValue(ProfileConstants.LastName, dto.LastName);
                member.SetValue(ProfileConstants.Phone, dto.Phone);
                member.SetValue(ProfileConstants.Department, dto.Department);

                var mediaId = member.GetValueOrDefault<int?>(ProfileConstants.Photo);

                if (dto.NewMedia.HasValue)
                {
                    member.SetValue(ProfileConstants.Photo, dto.NewMedia.Value);
                }

                if (dto.DeleteMedia)
                {
                    member.SetValue(ProfileConstants.Photo, null);
                }

                if ((dto.NewMedia.HasValue || dto.DeleteMedia) && mediaId.HasValue)
                {
                    var media = _mediaService.GetById(mediaId.Value);
                    if (media != null)
                        _mediaService.Delete(media);
                }

                _memberService.Save(member, false);

                UpdateMemberCache(dto.Id);
            }

            return isPresent;
        }

        public Guid Create(CreateMemberDto dto)
        {
            var fullName = $"{dto.FirstName} {dto.LastName}";
            var member = _memberService.CreateMember(dto.Email, dto.Email, fullName, "Member");
            member.SetValue(ProfileConstants.FirstName, dto.FirstName);
            member.SetValue(ProfileConstants.LastName, dto.LastName);
            member.SetValue(ProfileConstants.Phone, dto.Phone);
            member.SetValue(ProfileConstants.Department, dto.Department);
            member.SetValue(ProfileConstants.Photo, dto.MediaId);

            _memberService.Save(member, false);

            if (System.Web.Security.Roles.RoleExists(dto.Role.ToString()))
            {
                System.Web.Security.Roles.AddUserToRole(member.Username, dto.Role.ToString());
            }

            UpdateMemberCache(member.Key);

            return member.Key;
        }

        public Option<ReadMemberDto> Read(Guid id)
        {
            var member = Optional(_memberService.GetByKey(id));

            var dto = member.Map(mbr => new ReadMemberDto
            {
                LastName = mbr.GetValue<string>(ProfileConstants.LastName),
                FirstName = mbr.GetValue<string>(ProfileConstants.FirstName),
                Phone = mbr.GetValue<string>(ProfileConstants.Phone),
                Department = mbr.GetValue<string>(ProfileConstants.Department),
                Email = mbr.Email,
                Role = System.Web.Security.Roles.GetRolesForUser(mbr.Username)
                    .First()
                    .Apply(s => (IntranetRolesEnum)Enum.Parse(typeof(IntranetRolesEnum), s))
            });

            return dto;
        }

        public bool Delete(Guid id)
        {
            var member = _memberService.GetByKey(id);
            var isPresent = member != null;

            if (isPresent)
            {
                _memberService.Delete(member);
                DeleteFromCache(member.Key);
            }

            return isPresent;
        }

        protected virtual T GetFromSql(Guid id)
        {
            var member = _memberService.GetByKey(id);
            return member != null ? Map(member) : default;
        }

        protected virtual IEnumerable<T> GetAllFromSql()
        {
            var members = _memberService.GetAllMembers().Select(Map).ToList();
            return members;
        }

        protected virtual T Map(IMember member)
        {
            var relatedUserId = member.GetValueOrDefault<int?>(ProfileConstants.RelatedUser);
            var mappedMember = new T
            {
                Id = member.Key,
                Email = member.Email,
                LoginName = member.Username,
                Group = _intranetMemberGroupService.GetForMember(member.Id).First(),//todo do allow member to has more than one group?
                Inactive = member.IsLockedOut,
                RelatedUser = relatedUserId.HasValue ? _intranetUserService.GetUser(relatedUserId.Value) : null
            };
            mappedMember.IsSuperUser = mappedMember.RelatedUser != null && mappedMember.RelatedUser.IsSuperUser;

            string memberPhoto = null;
            var memberPhotoId = member.GetValueOrDefault<int?>(ProfileConstants.Photo) ?? member.GetMemberImageId(ProfileConstants.Photo);

            if (memberPhotoId.HasValue)
            {
                memberPhoto = _umbracoHelper.TypedMedia(memberPhotoId.Value)?.Url;
            }

            mappedMember.Photo = GetUserPhotoOrDefaultAvatar(memberPhoto);
            mappedMember.PhotoId = memberPhotoId;

            return mappedMember;
        }

        protected virtual IEnumerable<T> GetUnassignedToMemberUsers()
        {
            var members = GetAll();
            var assignedMembersIds = _memberService.GetAllMembers().Select(m => m.GetValue<Guid>("relatedUser"));
            var unassignedMembers = members.Join(assignedMembersIds, user => user.Id, id => id, (user, id) => user);

            return unassignedMembers;
        }
       

        protected virtual string GetUserPhotoOrDefaultAvatar(string userImage)
        {
            return !string.IsNullOrEmpty(userImage) ? userImage : string.Empty;
        }

        public virtual T GetByName(string name)
        {
            var members = GetAll();
            return members.SingleOrDefault(user => string.Equals(user.LoginName, name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual T GetByEmail(string email)
        {
            var members = GetAll();
            return members.SingleOrDefault(member => member.Email.ToLowerInvariant().Equals(email.ToLowerInvariant()));
        }

        public virtual void UpdateMemberCache(Guid memberId)
        {
            var updatedMember = GetFromSql(memberId);

            var allCachedMembers = GetAll().ToList();
            var oldCachedMember = allCachedMembers.Find(el => el.Id == memberId);

            if (oldCachedMember != null)
            {
                allCachedMembers.Remove(oldCachedMember);
            }

            if (updatedMember != null)
            {
                allCachedMembers.Add(updatedMember);
            }

            _cacheService.Set(MembersCacheKey, allCachedMembers, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual void UpdateMemberCache(IEnumerable<Guid> memberIds)
        {
            var allMembers = GetAllFromSql();
            var updatedMembers = allMembers.Join(memberIds, u => u.Id, id => id, (u, id) => u).ToList();

            var allCachedMembers = GetAll().ToList();
            var oldCachedMembers = allCachedMembers.Join(memberIds, u => u.Id, id => id, (u, id) => u).ToList();

            oldCachedMembers.ForEach(ocm =>
            {
                if (ocm != null)
                {
                    allCachedMembers.Remove(ocm);
                }
            });

            updatedMembers.ForEach(m =>
            {
                if (m != null)
                {
                    allCachedMembers.Add(m);
                }
            });

            _cacheService.Set(MembersCacheKey, allCachedMembers, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual void DeleteFromCache(Guid memberId)
        {
            var allCachedMembers = GetAll().ToList();
            var oldCachedMember = allCachedMembers.Find(el => el.Id == memberId);

            if (oldCachedMember != null)
            {
                allCachedMembers.Remove(oldCachedMember);
            }
            _cacheService.Set(MembersCacheKey, allCachedMembers, CacheHelper.GetMidnightUtcDateTimeOffset());
        }
    }
}
