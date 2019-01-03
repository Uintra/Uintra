using System;
using System.Linq;
using System.Web.Http;
using LanguageExt;
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
        public virtual IHttpActionResult Create(CreateUserDto createModel)
        {
            if (ModelState.IsValid)
            {
                var user = _intranetUserService.GetByEmail(createModel.Email);
                if (user != null)
                {
                    return Conflict();
                }

                var id = _intranetUserService.Create(createModel);

                return Ok(id);
            }
            else
            {
                return BadRequest(GetModelErrors());
            }
        }

        [HttpGet]
        public virtual IHttpActionResult Read(Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = _intranetUserService.Read(id);
                return result
                    .Match(
                        Some: dto => (IHttpActionResult)Ok(dto),
                        None: NotFound);
            }
            else
            {
                return BadRequest(GetModelErrors());
            }
        }

        [HttpPatch]
        public virtual IHttpActionResult Update(UpdateUserDto updateModel)
        {
            if (ModelState.IsValid)
            {
                return _intranetUserService.Update(updateModel)
                    ? (IHttpActionResult)Ok()
                    : NotFound();
            }
            else
            {
                return BadRequest(GetModelErrors());
            }
        }

        public virtual IHttpActionResult Delete(Guid id) =>

            _intranetUserService.Delete(id)
                ? (IHttpActionResult)Ok()
                : NotFound();


        private string GetModelErrors() =>
            ModelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                .Apply(errors => string.Join(Environment.NewLine, errors));
    }
}