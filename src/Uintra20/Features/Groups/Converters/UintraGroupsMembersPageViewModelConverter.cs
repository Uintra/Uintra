using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter : INodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        private readonly IGroupHelper _groupHelper;
        public UintraGroupsMembersPageViewModelConverter(IGroupHelper groupHelper)
        {
            _groupHelper = groupHelper;
        }

        public void Map(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.GroupHeader = _groupHelper.GetHeader(id);
        }
    }
}