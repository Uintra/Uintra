using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter : INodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupService _groupService;

        public UintraGroupsMembersPageViewModelConverter(
            IGroupLinkProvider groupLinkProvider,
            IGroupService groupService)
        {
            _groupLinkProvider = groupLinkProvider;
            _groupService = groupService;
        }

        public void Map(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;
            var canEdit = _groupService.CanEdit(id);
            viewModel.Links = _groupLinkProvider.GetGroupLinks(id, canEdit);
        }
    }
}