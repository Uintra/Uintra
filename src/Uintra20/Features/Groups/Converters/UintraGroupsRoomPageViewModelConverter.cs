using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter : INodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsRoomPageViewModelConverter(IGroupHelper groupHelper)
        {
            _groupHelper = groupHelper;
        }

        public void Map(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.GroupId = id;
            viewModel.GroupInfo = _groupHelper.GetGroupViewModel(id);
            viewModel.GroupHeader = _groupHelper.GetHeader(id);
        }
    }
}