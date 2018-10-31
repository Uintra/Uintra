using System;
using System.Web.Http;
using Uintra.Core.Attributes;
using Uintra.Core.User;
using Uintra.Core.User.DTO;

namespace Uintra.Users.Web
{
    [RoutePrefix("api/[controller]")]
    [ApiAuthorizationFilter]
    public abstract class MemberManagementApiControllerBase : ApiController
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected MemberManagementApiControllerBase(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }
        
        [HttpPost]
        public virtual Guid Create(CreateUserDto crateModel)
        {
            return _intranetUserService.Create(crateModel);
        }

        [HttpGet]
        public virtual ReadUserDto Read(Guid id)
        {
            return _intranetUserService.Read(id);
        }

        [HttpPatch]
        public virtual void Update(UpdateUserDto updateModel)
        {
            _intranetUserService.Update(updateModel);
        }

        [HttpDelete]
        public virtual void Delete(Guid id)
        {
            _intranetUserService.Delete(id);
        }
    }
}