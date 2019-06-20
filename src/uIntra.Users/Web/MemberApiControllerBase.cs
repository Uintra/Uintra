using System;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public abstract class MemberApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;

        protected MemberApiControllerBase(ICacheableIntranetMemberService cacheableIntranetMemberService)
        {
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
        }

        [HttpPost]
        public virtual void MemberChanged(Guid memberId)
        {
            _cacheableIntranetMemberService.UpdateMemberCache(memberId);
        }
    }
}
