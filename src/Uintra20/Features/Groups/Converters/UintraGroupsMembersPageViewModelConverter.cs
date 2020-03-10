using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        private readonly IGroupHelper _groupHelper;
        public UintraGroupsMembersPageViewModelConverter(IGroupHelper groupHelper, IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();

            viewModel.GroupHeader = _groupHelper.GetHeader(id);

            return OkResult();
        }
    }
}