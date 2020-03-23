using System.Linq;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Implementations
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

        public void MemberUpdateHandler(
            IMemberService sender,
            SaveEventArgs<IMember> @event)
        {
            foreach (var member in @event.SavedEntities)
            {
                // _cacheableIntranetMemberService.UpdateMemberCache(member.Id);
            }
        }

        public void MemberCreateHandler(
            IMemberService sender,
            SaveEventArgs<IMember> @event)
        {
            foreach (var member in @event.SavedEntities)
            {
                // _intranetMemberGroupService.AssignDefaultMemberGroup(member.Id);
                // _cacheableIntranetMemberService.UpdateMemberCache(member.Id);
            }
        }

        public void MemberDeleteHandler(
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

        public void MemberAssignedRolesHandler(
            IMemberService sender,
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId =>
            {
                _cacheableIntranetMemberService.UpdateMemberCache((int) memberId);
            });
        }

        public void MemberRemovedRolesHandler(
            IMemberService sender,
            RolesEventArgs @event)
        {
            @event.MemberIds.ForEach(memberId =>
            {
                _cacheableIntranetMemberService.UpdateMemberCache((int) memberId);
            });
        }
    }
}