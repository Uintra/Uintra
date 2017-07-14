using System;
using Umbraco.Core;

namespace Compent.uIntra.Installer
{
    public class MaxIntranetUsersCountVerificator : ApplicationEventHandler
    {
        private const int MaxIntranetUsersCount = 30;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var membersCount = memberService.Count();

            if (membersCount > MaxIntranetUsersCount)
            {
                throw new Exception(@"This is a beta version of uIntra. As result, this is only possible to have up to 30 members. 
If you are interested in more than 30 members, please don't hesitate and contact us by this email vma@compent.net");
            }
        }
    }
}