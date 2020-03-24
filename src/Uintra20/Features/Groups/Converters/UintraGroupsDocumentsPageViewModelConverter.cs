using System;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsDocumentsPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsDocumentsPageModel, UintraGroupsDocumentsPageViewModel>
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupDocumentsService _groupDocumentsService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;

        public UintraGroupsDocumentsPageViewModelConverter(
            IMediaHelper mediaHelper,
            IGroupDocumentsService groupDocumentsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
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

            viewModel.CanUpload = _groupDocumentsService.CanUpload(id.Value);
            viewModel.GroupHeader = _groupHelper.GetHeader(id.Value);
            viewModel.GroupId = id.Value;

            return OkResult();
        }
    }
}