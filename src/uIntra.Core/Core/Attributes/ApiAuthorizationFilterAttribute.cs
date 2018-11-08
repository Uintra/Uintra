using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;
using LanguageExt;
using Uintra.Core.ApplicationSettings;
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
        private readonly IApplicationSettings _applicationSettings;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public ApiAuthorizationFilterAttribute()
        {
            _intranetUserService = DependencyResolver.Current.GetService<IIntranetUserService<IIntranetUser>>();
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
            var relatedUserWithWebMasterRole = Optional(_intranetUserService.GetByEmail(mail))
                .Filter(member => member.Role.Name == IntranetRolesEnum.WebMaster.ToString())
                .Bind(member => member.UmbracoId.ToOption())
                .Map(id => _memberService.GetById(id));

            Option<IMember> GetUserWithMatchingEmail () => Optional(_memberService.GetByEmail(mail));

            return EnumerableExtensions
                .Choose(relatedUserWithWebMasterRole, GetUserWithMatchingEmail)
                .Map(user => Membership.ValidateUser(user.Username, password))
                .IfNone(() => false);
        }
    }
}