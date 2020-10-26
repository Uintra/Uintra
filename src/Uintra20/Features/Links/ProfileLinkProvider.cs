using System;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.User;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Links
{
    public class ProfileLinkProvider : IProfileLinkProvider
    {
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;

        public ProfileLinkProvider(IIntranetUserContentProvider intranetUserContentProvider)
        {
            _intranetUserContentProvider = intranetUserContentProvider;
        }

        public UintraLinkModel GetProfileLink(IIntranetMember member) =>
            GetProfileLink(member.Id);

        public UintraLinkModel GetProfileLink(Guid userId) =>
            _intranetUserContentProvider.GetProfilePage()?.Url?.AddIdParameter(userId).ToLinkModel();
    }
}