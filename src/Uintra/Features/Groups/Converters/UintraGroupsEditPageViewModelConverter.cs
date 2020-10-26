using System;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Models;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Media.Enums;
using Uintra.Features.Media.Helpers;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Groups.Converters
{
    public class UintraGroupsEditPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsEditPageModel, UintraGroupsEditPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        public UintraGroupsEditPageViewModelConverter(
            IGroupService groupService,
            IMediaHelper mediaHelper,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _groupService = groupService;
            _mediaHelper = mediaHelper;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsEditPageModel node, UintraGroupsEditPageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!groupId.HasValue) return NotFoundResult();

            var group = _groupService.Get(groupId.Value);

            if (group == null || group.IsHidden) return NotFoundResult();

            if (!_groupService.CanEdit(groupId.Value)) return ForbiddenResult();

            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;
            viewModel.Info = _groupHelper.GetInfoViewModel(groupId.Value);
            viewModel.GroupHeader = _groupHelper.GetHeader(groupId.Value);
            viewModel.GroupId = groupId.Value;

            return OkResult();
        }
    }
}