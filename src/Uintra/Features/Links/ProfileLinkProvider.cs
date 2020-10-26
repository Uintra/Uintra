using System;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.User;
using Uintra.Features.Links.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Links
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