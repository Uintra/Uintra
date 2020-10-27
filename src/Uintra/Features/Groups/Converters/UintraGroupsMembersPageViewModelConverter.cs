using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Models;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        private readonly IGroupService _groupService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        public UintraGroupsMembersPageViewModelConverter(
            IGroupService groupService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _groupService = groupService;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!groupId.HasValue) return NotFoundResult();

            var group = _groupService.Get(groupId.Value);

            if (group == null || group.IsHidden) return NotFoundResult();

            viewModel.GroupHeader = _groupHelper.GetHeader(groupId.Value);

            return OkResult();
        }
    }
}