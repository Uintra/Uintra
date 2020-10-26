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
    public class UintraGroupsDocumentsPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupDocumentsService _groupDocumentsService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;

        public UintraGroupsDocumentsPageViewModelConverter(
            IGroupService groupService,
            IMediaHelper mediaHelper,
            IGroupDocumentsService groupDocumentsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _groupService = groupService;
            _mediaHelper = mediaHelper;
            _groupDocumentsService = groupDocumentsService;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsDocumentsPageModel node, UintraGroupsDocumentsPageViewModel viewModel)
        {
            var settings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent);

            viewModel.AllowedMediaExtensions = settings?.AllowedMediaExtensions;

            var id = _context.ParseQueryString("groupId").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var group = _groupService.Get(id.Value);

            if (group == null || group.IsHidden) return NotFoundResult();

            viewModel.CanUpload = _groupDocumentsService.CanUpload(id.Value);
            viewModel.GroupHeader = _groupHelper.GetHeader(id.Value);
            viewModel.GroupId = id.Value;

            return OkResult();
        }
    }
}