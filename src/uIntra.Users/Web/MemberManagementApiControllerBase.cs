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
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected MemberManagementApiControllerBase(IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }


        [HttpPost]
        public virtual IHttpActionResult Create(CreateMemberDto createModel)
        {
            if (ModelState.IsValid)
            {
                var user = _intranetMemberService.GetByEmail(createModel.Email);
                if (user != null)
                {
                    return Conflict();
                }

                var id = _intranetMemberService.Create(createModel);

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
                var result = _intranetMemberService.Read(id);
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
        public virtual IHttpActionResult Update(UpdateMemberDto updateModel)
        {
            if (ModelState.IsValid)
            {
                return _intranetMemberService.Update(updateModel)
                    ? (IHttpActionResult)Ok()
                    : NotFound();
            }
            else
            {
                return BadRequest(GetModelErrors());
            }
        }

        public virtual IHttpActionResult Delete(Guid id) =>

            _intranetMemberService.Delete(id)
                ? (IHttpActionResult)Ok()
                : NotFound();


        private string GetModelErrors() =>
            ModelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                .Apply(errors => string.Join(Environment.NewLine, errors));
    }
}