using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMemberDeletingEventService
    {
        void MemberDeleteHandler(IMemberService sender, DeleteEventArgs<IMember> args);
    }
}