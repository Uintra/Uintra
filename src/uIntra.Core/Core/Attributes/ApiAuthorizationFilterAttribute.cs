using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;
using Uintra.Core.ApplicationSettings;
using Umbraco.Core.Services;

namespace Uintra.Core.Attributes
{
    public class ApiAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        private readonly IMemberService _memberService;
        private readonly IApplicationSettings _applicationSettings;

        public ApiAuthorizationFilterAttribute()
        {
            _applicationSettings = DependencyResolver.Current.GetService<IApplicationSettings>();
            _memberService = DependencyResolver.Current.GetService<IMemberService>();
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                var originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));
                var originalStringValues = originalString.Split(':');
                var mail = originalStringValues[0];
                var password = originalStringValues[1];


                if (!IsCredentialsValid(mail, password))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            base.OnAuthorization(actionContext);
        }

        private bool IsCredentialsValid(string mail, string password)
        {
            if (_applicationSettings.MemberApiAuthentificationEmail != mail) return false;
            var member = _memberService.GetByEmail(mail);
            return Membership.ValidateUser(member.Username, password);
        }
    }
}