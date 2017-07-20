using uIntra.Core.Exceptions;
using Umbraco.Core;

namespace uIntra.Core.Handlers
{
    public class MaximumUsersCountVerificator : ApplicationEventHandler
    {
        private const int MaxIntranetUsersCount = 30;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var membersCount = memberService.Count();

            if (false && membersCount > MaxIntranetUsersCount)
            {
                throw new MaximumUsersCountException($"This is a beta version of uIntra. As result, this is only possible to have up to {MaxIntranetUsersCount} members." +
                                    $" If you are interested in more than {MaxIntranetUsersCount} members, please don't hesitate and contact us by this email vma@compent.net");
            }
        }
    }
}