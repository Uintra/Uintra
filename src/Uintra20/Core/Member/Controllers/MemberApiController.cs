using System;
using System.Linq;
using System.Web.Http;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Features.Permissions.Interfaces;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace Uintra20.Core.Member.Controllers
{
    public class MemberApiController : UmbracoAuthorizedApiController
    {
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IIntranetMemberGroupService _memberGroupService;
        private readonly IMemberService _memberService;

        public MemberApiController(
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IIntranetMemberGroupService memberGroupService,
            IMemberService memberService)
        {
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberGroupService = memberGroupService;
            _memberService = memberService;
        }

        [HttpPost]
        public virtual bool MemberChanged(Guid memberId)
        {
            var member = _memberService.GetByKey(memberId);
            var groups = _memberService.GetAllRoles(member.Id).ToList();

            if (!groups.Any())
            {
                _memberGroupService.AssignDefaultMemberGroup(member.Id);
            }

            if (groups.Count > 1)
            {
                _memberGroupService.RemoveFromAll(member.Id);
                _memberGroupService.AssignDefaultMemberGroup(member.Id);
            }

            _cacheableIntranetMemberService.UpdateMemberCache(memberId);
            return true;
        }
    }
}