using System;
using Uintra20.Core.Extensions;
using Uintra20.Core.User;

namespace Uintra20.Core.Links
{
    public class ProfileLinkProvider : IProfileLinkProvider
    {
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;

        public ProfileLinkProvider(IIntranetUserContentProvider intranetUserContentProvider)
        {
            _intranetUserContentProvider = intranetUserContentProvider;
        }

        public string GetProfileLink(IIntranetMember member) =>
            GetProfileLink(member.Id);

        public string GetProfileLink(Guid userId) =>
            _intranetUserContentProvider.GetProfilePage().Url.AddIdParameter(userId);
    }
}