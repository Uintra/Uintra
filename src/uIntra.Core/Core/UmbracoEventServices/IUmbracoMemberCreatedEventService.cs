using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMemberCreatedEventService
    {
        void ProcessMemberCreated(IMemberService sender, NewEventArgs<IMember> args);
    }
}
