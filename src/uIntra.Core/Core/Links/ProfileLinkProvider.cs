using System;
using uIntra.Core.Extentions;
using uIntra.Core.User;

namespace uIntra.Core.Links
{
    public class ProfileLinkProvider : IProfileLinkProvider
    {
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;

        public ProfileLinkProvider(IIntranetUserContentHelper intranetUserContentHelper)
        {
            _intranetUserContentHelper = intranetUserContentHelper;
        }

        public string GetProfileLink(IIntranetUser user) => 
            GetProfileLink(user.Id);

        public string GetProfileLink(Guid userId) => 
            _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(userId);
    }
}