using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        public UintraGroupsMembersPageViewModelConverter(
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!groupId.HasValue) return NotFoundResult();

            viewModel.GroupHeader = _groupHelper.GetHeader(groupId.Value);

            return OkResult();
        }
    }
}