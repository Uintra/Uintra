using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models.Dto;
using Uintra20.Core.User;
using Uintra20.Core.User.Models;
using Uintra20.Features.MemberProfile;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using CacheHelper = Uintra20.Infrastructure.Caching.CacheHelper;


namespace Uintra20.Core.Member.Services
{
    public abstract class IntranetMemberServiceBase<T> : IIntranetMemberService<T>, ICacheableIntranetMemberService
          where T : class, IIntranetMember, new()
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string MembersCacheKey => "IntranetMembersCache";
        private const string GroupWebMaster = "WebMaster";

        private readonly IMediaService _mediaService;
        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ICacheService _cacheService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        protected IntranetMemberServiceBase(
            IMediaService mediaService,
            IMemberService memberService,
            UmbracoContext umbracoContext,
            UmbracoHelper umbracoHelper,
            ICacheService cacheService,
            IIntranetUserService<IntranetUser> intranetUserService,
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

        public virtual async Task<bool> IsCurrentMemberSuperUserAsync()
        {
            var currentMember = await GetCurrentMemberAsync();
            return currentMember != null && currentMember.IsSuperUser;
        }

        public virtual async Task<T> GetAsync(IHaveOwner model) => await GetAsync(model.OwnerId);

        public virtual async Task<T> GetAsync(Guid id) => await GetSingleAsync(el => el.Id == id);

        public virtual async Task<T> GetAsync(int id)
        {
            var member = (await GetAllAsync()).SingleOrDefault(el => el.UmbracoId == id);
            if (member == null)
                member = await MapAsync(_memberService.GetById(id));
            return member;
        }

        public virtual async Task<T> GetByUserIdAsync(int userId)
        {
            return await GetSingleAsync(el => el.RelatedUser?.Id == userId);
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(await GetAllAsync(),
                x => x,
                member => member.Id,
                (_, member) => member);
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(IEnumerable<int> ids)
        {
            return ids.Distinct().Join(await GetAllAsync(),
                id => id,
                member => member.RelatedUser?.Id,
                (id, member) => member);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var members = await _cacheService.GetOrSetAsync(async () => (await GetAllFromSqlAsync()).ToList(), MembersCacheKey, CacheHelper.GetMidnightUtcDateTimeOffset());
            return members;
        }

        public virtual async Task<T> GetCurrentMemberAsync()
        {
            var member = _umbracoHelper.MembershipHelper.GetCurrentMember();
            if (member != null) return await GetAsync(member.Id);

            var umbracoUser = _umbracoContext.Security.CurrentUser;
            if (umbracoUser != null) return await GetByUserIdAsync(umbracoUser.Id);

            return default(T);
        }

        public virtual async Task<IEnumerable<T>> GetByGroupAsync(int memberGroupId)
        {
            var members = (await GetAllAsync()).Where(el => el.Groups.Select(g => g.Id).Contains(memberGroupId));
            return members;
        }

        public virtual async Task<bool> UpdateAsync(UpdateMemberDto dto)
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

                await UpdateMemberCacheAsync(dto.Id);
            }

            return isPresent;
        }

        public virtual async Task<Guid> CreateAsync(CreateMemberDto dto)
        {
            var fullName = $"{dto.FirstName} {dto.LastName}";
            var member = _memberService.CreateMember(dto.Email, dto.Email, fullName, "Member");
            member.SetValue(ProfileConstants.FirstName, dto.FirstName);
            member.SetValue(ProfileConstants.LastName, dto.LastName);
            member.SetValue(ProfileConstants.Phone, dto.Phone);
            member.SetValue(ProfileConstants.Department, dto.Department);
            member.SetValue(ProfileConstants.Photo, dto.MediaId);

            _memberService.Save(member, false);


            await UpdateMemberCacheAsync(member.Key);

            return member.Key;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var member = _memberService.GetByKey(id);

            if (member != null)
            {
                _memberService.Delete(member);
                await DeleteFromCacheAsync(member.Key);
            }

            return member != null;
        }

        public virtual async Task<T> GetByNameAsync(string name)
        {
            var members = await GetAllAsync();
            return members.SingleOrDefault(user => string.Equals(user.LoginName, name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual async Task<T> GetByEmailAsync(string email)
        {
            var members = await GetAllAsync();
            var normalizedEmail = email.ToLowerInvariant();
            return members.SingleOrDefault(member => member.Email.ToLowerInvariant().Equals(normalizedEmail));
        }

        public virtual async Task UpdateMemberCacheAsync(int memberId)
        {
            var member = await GetAsync(memberId);
            await UpdateMemberCacheAsync(member.Id);
        }

        public virtual async Task UpdateMemberCacheAsync(Guid memberId)
        {
            var updatedMember = await GetFromSqlOrNoneAsync(memberId);
            var allCachedMembers = await GetAllAsync();

            IEnumerable<T> updatedCache;

            if (updatedMember != null)
            {
                updatedCache = allCachedMembers.WithUpdatedElement(el => el.Id == memberId, updatedMember);
            }
            else
            {
                updatedCache = allCachedMembers.Where(el => el.Id != memberId);
            }

            await _cacheService.SetAsync(() => Task.FromResult(updatedCache.ToList()), MembersCacheKey, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual async Task UpdateMemberCacheAsync(IEnumerable<Guid> memberIds)
        {
            var allCachedMembers = await GetAllAsync();

            foreach (var memberId in memberIds)
            {
                IEnumerable<T> updatedCache;
                T updatedMember = await GetFromSqlOrNoneAsync(memberId);

                if (updatedMember != null)
                {
                    updatedCache = allCachedMembers.WithUpdatedElement(el => el.Id == memberId, updatedMember);
                }
                else
                {
                    updatedCache = allCachedMembers.Where(el => el.Id != memberId);
                }
            }

            await _cacheService.SetAsync(() => Task.FromResult(allCachedMembers.ToList()), MembersCacheKey, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        public virtual async Task DeleteFromCacheAsync(Guid memberId)
        {
            await _cacheService.SetAsync(async () => (await GetAllAsync()).Where(el => el.Id != memberId).ToList(), MembersCacheKey, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        protected virtual async Task<T> GetFromSqlOrNoneAsync(Guid id) =>
            await MapAsync(_memberService.GetByKey(id));

        protected virtual async Task<IEnumerable<T>> GetAllFromSqlAsync() =>
            await _memberService
                .GetAllMembers()
                .SelectAsync(MapAsync);

        private async Task<T> GetSingleAsync(Func<T, bool> predicate)
        {
            var member = (await GetAllAsync()).Single(predicate);
            return member;
        }

        //#endregion

        #region sync

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

        private T GetSingle(Func<T, bool> predicate)
        {
            var member = GetAll();
            var result = member.Single(predicate);

            return result;
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<Guid> ids)
        {
            return ids.Distinct().Join(GetAll(),
                x => x,
                member => member.Id,
                (_, member) => member);
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
            var members = _cacheService.GetOrSet(MembersCacheKey, () => GetAllFromSql().ToList(), CacheHelper.GetMidnightUtcDateTimeOffset());
            return members;
        }

        public virtual T GetCurrentMember()
        {
            var member = _umbracoHelper.MembershipHelper.GetCurrentMember();
            if (member != null) return Get(member.Key);
            
            var umbracoUser = _umbracoContext.Security.CurrentUser;
            if (umbracoUser != null) return GetByUserId(umbracoUser.Id);

            return default(T);
        }
        public virtual T GetByUserId(int userId)
        {
            return GetSingle(el => el.RelatedUser?.Id == userId);
        }


        public virtual IEnumerable<T> GetByGroup(int memberGroupId)
        {
            var members = GetAll().Where(el => el.Groups.Select(g => g.Id).Contains(memberGroupId));
            return members;
        }

		public virtual bool Update(UpdateMemberDto dto)
		{
			var member = _memberService.GetByKey(dto.Id);
            if (member == null) 
                return false;

            member.SetValue(ProfileConstants.FirstName, dto.FirstName);
            member.SetValue(ProfileConstants.LastName, dto.LastName);
            member.SetValue(ProfileConstants.Phone, dto.Phone);
            member.SetValue(ProfileConstants.Department, dto.Department);

            var mediaId = member.GetValueOrDefault<int?>(ProfileConstants.Photo);

            if (dto.NewMedia.HasValue)
                member.SetValue(ProfileConstants.Photo, dto.NewMedia.Value);

            if (dto.DeleteMedia)
                member.SetValue(ProfileConstants.Photo, null);

            if ((dto.NewMedia.HasValue || dto.DeleteMedia) && mediaId.HasValue)
            {
                var media = _mediaService.GetById(mediaId.Value);
                if (media != null)
                    _mediaService.Delete(media);
            }

            _memberService.Save(member, false);

            UpdateMemberCache(dto.Id);

            return true;
		}

        public virtual Guid Create(CreateMemberDto dto)
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

        public ReadMemberDto Read(Guid id)
        {
            var member = _memberService.GetByKey(id);

			if (member == null)			
				return null;
			

			var dto = new ReadMemberDto
			{
				LastName = member.GetValue<string>(ProfileConstants.LastName),
				FirstName = member.GetValue<string>(ProfileConstants.FirstName),
				Phone = member.GetValue<string>(ProfileConstants.Phone),
				Department = member.GetValue<string>(ProfileConstants.Department),
				Email = member.Email
			};

            return dto;
        }

        public bool Delete(Guid id)
        {
            var member = _memberService.GetByKey(id);

            if (member == null) 
                return false;

            _memberService.Delete(member);
            DeleteFromCache(member.Key);

            return true;
		}

        protected virtual T GetFromSqlOrNone(Guid id) =>
            Map(_memberService.GetByKey(id));

        protected virtual IEnumerable<T> GetAllFromSql() =>
            _memberService
                .GetAllMembers()
                .Select(Map);

        protected virtual async Task<T> MapAsync(IMember member)
        {
            var relatedUserId = member.GetValueOrDefault<int>(ProfileConstants.RelatedUser);
            var photo = member.GetValueOrDefault<int?>(ProfileConstants.Photo);


            var relatedUser = await _intranetUserService.GetByIdAsync(relatedUserId);

            var memberPhotoId = photo ?? member.GetMemberImageId(ProfileConstants.Photo);

            var memberPhotoUrl = _umbracoHelper.Media(memberPhotoId)?.Url;

            var memberGroups = _intranetMemberGroupService.GetForMember(member.Id).ToArray();

            var mappedMember = new T
            {
                Id = member.Key,
                Email = member.Email,
                LoginName = member.Username,
                Groups = memberGroups,
                Inactive = member.IsLockedOut,
                RelatedUser = relatedUser,
                IsSuperUser = relatedUser.IsSuperUser && memberGroups.Any(g => g.Name is GroupWebMaster),
                Photo = GetUserPhotoOrDefaultAvatar(memberPhotoUrl),
                PhotoId = memberPhotoId,
                UmbracoId = member.Id
            };

            return mappedMember;
        }

        protected virtual T Map(IMember member)
        {
            var relatedUserId = member.GetValueOrDefault<int>(ProfileConstants.RelatedUser);
            var photo = member.GetValueOrDefault<int?>(ProfileConstants.Photo);

            var relatedUser = _intranetUserService.GetById(relatedUserId);

            var memberPhotoId = photo ?? member.GetMemberImageId(ProfileConstants.Photo);

            var memberPhotoUrl = _umbracoHelper.Media(memberPhotoId)?.Url;

            var memberGroups = _intranetMemberGroupService.GetForMember(member.Id).ToArray();

            var mappedMember = new T
            {
                Id = member.Key,
                Email = member.Email,
                LoginName = member.Username,
                Groups = memberGroups,
                Inactive = member.IsLockedOut,
                RelatedUser = relatedUser,
                IsSuperUser = relatedUser != null && relatedUser.IsSuperUser && memberGroups.Any(g => g.Name is GroupWebMaster),
                Photo = GetUserPhotoOrDefaultAvatar(memberPhotoUrl),
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


            var updatedCache = updatedMember != null
                ? allCachedMembers.WithUpdatedElement(el => el.Id == memberId, updatedMember)
                : allCachedMembers.Where(el => el.Id != memberId);

			_cacheService.Set(MembersCacheKey, updatedCache.ToArray(), CacheHelper.GetMidnightUtcDateTimeOffset());
		}

		public virtual void UpdateMemberCache(IEnumerable<Guid> memberIds)
		{
			var allCachedMembers = GetAll()
                .ToArray();

            var updatedMembers = memberIds
                .SelectMany(id =>
                {
                    var member = GetFromSqlOrNone(id);
                    return member != null
                        ? allCachedMembers.WithUpdatedElement(el => el.Id == id, member)
                        : allCachedMembers.Where(el => el.Id != id);
                })
                .ToArray();

			_cacheService.Set(MembersCacheKey, updatedMembers, CacheHelper.GetMidnightUtcDateTimeOffset());
		}

        public virtual void DeleteFromCache(Guid memberId)
        {
            var updatedCache = GetAll().Where(el => el.Id != memberId).ToList();
            _cacheService.Set(MembersCacheKey, updatedCache, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        #endregion
    }
}