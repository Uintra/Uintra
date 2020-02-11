using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsEditPageViewModelConverter : INodeViewModelConverter<UintraGroupsEditPageModel, UintraGroupsEditPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly ILightboxHelper _lightboxHelper;

        public UintraGroupsEditPageViewModelConverter(IGroupService groupService, ILightboxHelper lightboxHelper)
        {
            _lightboxHelper = lightboxHelper;
            _groupService = groupService;
        }

        public void Map(UintraGroupsEditPageModel node, UintraGroupsEditPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.Info = GetInfo(id);
        }

        public GroupInfoViewModel GetInfo(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            var groupInfo = group.Map<GroupInfoViewModel>();

            if (group.ImageId.HasValue)
            {
                _lightboxHelper.FillGalleryPreview(groupInfo, Enumerable.Repeat(group.ImageId.Value, 1));
            }

            return groupInfo;
        }
    }
}