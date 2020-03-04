using System;
using System.Web;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsMembersPageViewModelConverter : UintraRestrictedNodeViewModelConverter<UintraGroupsMembersPageModel, UintraGroupsMembersPageViewModel>
    {
        public UintraGroupsMembersPageViewModelConverter(IErrorLinksService errorLinksService)
        : base (errorLinksService)
        {
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsMembersPageModel node, UintraGroupsMembersPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();

            viewModel.GroupId = id;

            return OkResult();
        }
    }
}