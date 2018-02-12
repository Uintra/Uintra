using System;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public abstract class MemberApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly ICacheableIntranetUserService _cacheableIntranetUserService;

        protected MemberApiControllerBase(ICacheableIntranetUserService cacheableIntranetUserService)
        {
            _cacheableIntranetUserService = cacheableIntranetUserService;
        }

        [HttpPost]
        public virtual void MemberChanged(Guid memberId)
        {
            _cacheableIntranetUserService.UpdateUserCache(memberId);
        }
    }
}
