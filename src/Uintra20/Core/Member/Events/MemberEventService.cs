using System.Linq;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.UmbracoEvents.Services;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.Member.Events
{
    public class MemberEventService : 
        IUmbracoMemberDeletingEventService, 
        IUmbracoMemberCreatedEventService,
        IUmbracoMemberAssignedRolesEventService,
        IUmbracoMemberRemovedRolesEventService
    {
        private readonly ICacheableIntranetMemberService _cacheableIntranetMemberService;
        private readonly IMemberService _memberService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        public MemberEventService(
            ICacheableIntranetMemberService cacheableIntranetMemberService,
            IMemberService memberService,
            IIntranetMemberGroupService intranetMemberGroupService
            )
        {
            _cacheableIntranetMemberService = cacheableIntranetMemberService;
            _memberService = memberService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }

        public void ProcessMemberCreated(
            IMemberService sender, 
            SaveEventArgs<IMember> @event)
        {
            foreach (var member in @event.SavedEntities)
            {
                _intranetMemberGroupService.AssignDefaultMemberGroup(member.Id);
                _cacheableIntranetMemberService.UpdateMemberCache(member.Id);
            }
        }

        public void ProcessMemberDeleting(
            IMemberService sender, 
            DeleteEventArgs<IMember> @event)
        {
            foreach (var member in @event.DeletedEntities)
            {
                member.IsLockedOut = true;
                _memberService.Save(member);
                _cacheableIntranetMemberService.UpdateMemberCache(member.Key);
            }

            if (@event.CanCancel) @event.Cancel = true;
        }

        public void ProcessMemberAssignedRoles(
            IMemberService sender, 
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId => _cacheableIntranetMemberService.UpdateMemberCache(memberId));
        }

        public void ProcessMemberRemovedRoles(
            IMemberService sender, 
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId =>
            {
                var groups = _memberService.GetAllRoles(memberId);

                if (!groups.Any()) _intranetMemberGroupService.AssignDefaultMemberGroup(memberId);

                _cacheableIntranetMemberService.UpdateMemberCache(memberId);
            });
        }
    }
}
