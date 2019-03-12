using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;
using LanguageExt;
using Uintra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Uintra.Core.Extensions;
using static LanguageExt.Prelude;

namespace Uintra.Core.Attributes
{
    public class ApiAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        private readonly IMemberService _memberService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public ApiAuthorizationFilterAttribute()
        {
            _intranetMemberService = DependencyResolver.Current.GetService<IIntranetMemberService<IIntranetMember>>();
            _memberService = DependencyResolver.Current.GetService<IMemberService>();
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization?.Parameter is null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                var originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));
                var originalStringValues = originalString.Split(':');
                if (originalStringValues.Length == 2)
                {
                    var mail = originalStringValues[0];
                    var password = originalStringValues[1];


                    if (!IsCredentialsValid(mail, password))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }

            base.OnAuthorization(actionContext);
        }

        private bool IsCredentialsValid(string mail, string password)
        {
            var relatedUserWithSuperUserPermissions = Optional(_intranetMemberService.GetByEmail(mail))
                .Bind(member => member.RelatedUser.Filter(user => user.IsSuperUser))
                .Map(user => _memberService.GetById(user.Id));

            Option<IMember> GetUserWithMatchingEmail() => Optional(_memberService.GetByEmail(mail));

            return relatedUserWithSuperUserPermissions
                .Choose(GetUserWithMatchingEmail)
                .Map(user => Membership.ValidateUser(user.Username, password))
                .IfNone(() => false);
        }
    }
}