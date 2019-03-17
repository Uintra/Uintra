using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
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
        private readonly ICacheService _cacheService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        protected IntranetMemberServiceBase(
            IMediaService mediaService,
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _mediaService = mediaService;
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
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

        public virtual T Get(int id)
        {
            var member = GetAll().SingleOrDefault(el => el.UmbracoId == id);
            if (member == null)
                member = Map(_memberService.GetById(id));
            return member;
        }

        public virtual T GetByUserId(int userId)
        {
            return GetSingle(el => el.RelatedUser.Map(u => u.Id) == userId);
        }

        private T GetSingle(Func<T, bool> predicate)
        {
            var member = GetAll().Single(predicate);
            return member;
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(GetAll(),
                identity,
                member => member.Id,
                (_, member) => member);
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<int> ids)
        {
            return ids.Distinct().Join(GetAll(),
                 id => id,
                member => member.RelatedUser.Map(x=>x.Id),
                 (id, member) => member);
        }

        public virtual IEnumerable<T> GetAll()
        {
            var members = _cacheService.GetOrSet(MembersCacheKey, ()=> GetAllFromSql().ToArray(), CacheHelper.GetMidnightUtcDateTimeOffset()).ToList();
            return members;
        }

        public virtual T GetCurrentMember()
        {
            var member = _umbracoHelper.MembershipHelper.GetCurrentMember();
            if (member != null) return Get(member.GetKey());

            var umbracoUser = _umbracoContext.Security.CurrentUser;
            if (umbracoUser != null) return GetByUserId(umbracoUser.Id);

            return default;
        }

        public virtual IEnumerable<T> GetByGroup(int memberGroupId)
        {
            var members = GetAll().Where(el => el.Group.Id == memberGroupId);
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
                Email = mbr.Email
            });

            return dto;
        }

        public bool Delete(Guid id)
        {
            var member = Optional(_memberService.GetByKey(id));

            member.Do(mbr =>
            {
                _memberService.Delete(mbr);
                DeleteFromCache(mbr.Key);
            });

            return member.IsSome;
        }

        protected virtual Option<T> GetFromSqlOrNone(Guid id) => 
            Optional(_memberService.GetByKey(id))
                .Map(Map);

        protected virtual IEnumerable<T> GetAllFromSql() =>
            _memberService
                .GetAllMembers()
                .Select(Map);

        protected virtual T Map(IMember member)
        {
            var relatedUserId = member.GetValueOrDefault<int?>(ProfileConstants.RelatedUser).ToOption();
            var relatedUser = relatedUserId.Map(id => _intranetUserService.GetUser(id));
           
            var memberPhotoId = member
                .GetValueOrDefault<int?>(ProfileConstants.Photo).ToOption()
                .Choose(() => member.GetMemberImageId(ProfileConstants.Photo));

            var memberPhotoUrl = memberPhotoId
                .Bind(id => Optional(_umbracoHelper.TypedMedia(id)?.Url));

            var mappedMember = new T
            {
                Id = member.Key,
                Email = member.Email,
                LoginName = member.Username,
                Group = _intranetMemberGroupService.GetForMember(member.Id).FirstOrDefault(), //todo do allow member to has more than one group?
                Inactive = member.IsLockedOut,
                RelatedUser = relatedUser,
                IsSuperUser = relatedUser.Match(Some: user => user.IsSuperUser, None: () => false),
                Photo = memberPhotoUrl.Map(GetUserPhotoOrDefaultAvatar),
                PhotoId = memberPhotoId,
                UmbracoId = member.Id
            };

            return mappedMember;
        }


        protected virtual string GetUserPhotoOrDefaultAvatar(string userImage) => 
            !string.IsNullOrEmpty(userImage) ? userImage : string.Empty;

        public virtual T GetByName(string name)
        {
            var members = GetAll();
            return members.SingleOrDefault(user => string.Equals(user.LoginName, name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual T GetByEmail(string email)
        {
            var members = GetAll();
            var normalizedEmail = email.ToLowerInvariant();
            return members.SingleOrDefault(member => member.Email.ToLowerInvariant().Equals(normalizedEmail));
        }

        public virtual void UpdateMemberCache(int memberId)
        {
            var member = Get(memberId);
            UpdateMemberCache(member.Id);
        }

        public virtual void UpdateMemberCache(Guid memberId)
        {
            var updatedMember = GetFromSqlOrNone(memberId);
            var allCachedMembers = GetAll();

            var updatedCache = updatedMember.Match(
                Some: member => allCachedMembers.WithUpdatedElement(el => el.Id == memberId, member),
                None: () => allCachedMembers.Where(el => el.Id != memberId))
                .ToList();
          
            _cacheService.Set(MembersCacheKey, updatedCache, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual void UpdateMemberCache(IEnumerable<Guid> memberIds) => 
            _cacheService.Set(MembersCacheKey, GetAllFromSql().ToArray(), CacheHelper.GetMidnightUtcDateTimeOffset());

        public virtual void DeleteFromCache(Guid memberId)
        {
            var updatedCache = GetAll().Where(el => el.Id != memberId).ToList();
            _cacheService.Set(MembersCacheKey, updatedCache, CacheHelper.GetMidnightUtcDateTimeOffset());
        }
    }
}
