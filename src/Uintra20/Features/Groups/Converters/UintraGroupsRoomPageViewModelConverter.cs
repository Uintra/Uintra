using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter : INodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupService _groupService;

        public UintraGroupsRoomPageViewModelConverter(
            IGroupLinkProvider groupLinkProvider,
            IGroupService groupService)
        {
            _groupLinkProvider = groupLinkProvider;
            _groupService = groupService;
        }

        public void Map(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;
            var canEdit = _groupService.CanEdit(id);
            viewModel.Links = _groupLinkProvider.GetGroupLinks(id, canEdit);
        }
    }
}