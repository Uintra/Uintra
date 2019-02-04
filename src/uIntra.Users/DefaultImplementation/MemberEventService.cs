using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Users
{
    public class MemberEventService : IUmbracoMemberDeletingEventService
    {
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IMemberService _memberService;

        public MemberEventService(ICacheableIntranetMemberService cacheableIntranetMemberService, IMemberService memberService)
        {
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberService = memberService;
        }

        public void ProcessMemberDeleting(IMemberService sender, DeleteEventArgs<IMember> args)
        {
            foreach (var member in args.DeletedEntities)
            {
                member.IsLockedOut = true;
                _memberService.Save(member);
                _cacheableIntranetMemberService.UpdateMemberCache(member.Key);
            }

            if (args.CanCancel)
            {
                args.Cancel = true;
            }
        }
    }
}
