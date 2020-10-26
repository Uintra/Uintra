using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMemberCreatedEventService
    {
        void MemberCreateHandler(IMemberService sender, SaveEventArgs<IMember> args);
        void MemberUpdateHandler(IMemberService sender, SaveEventArgs<IMember> args);
    }
}
