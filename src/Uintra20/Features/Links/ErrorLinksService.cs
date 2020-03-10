using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Shared.PageNotFoundPage;
using Uintra20.Core.UbaselineModels;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Links
{
    public class ErrorLinksService : IErrorLinksService
    {
        private readonly INodeModelService _nodeModelService;

        public ErrorLinksService(INodeModelService nodeModelService)
        {
            _nodeModelService = nodeModelService;
        }

        public UintraLinkModel GetNotFoundPageLink()
        {
            var notFoundPage = _nodeModelService.AsEnumerable().OfType<PageNotFoundPageModel>().Single();

            return notFoundPage.Url.ToLinkModel();
        }

        public UintraLinkModel GetForbiddenPageLink()
        {
            var forbiddenPage = _nodeModelService.AsEnumerable().OfType<ForbiddenPageModel>().Single();

            return forbiddenPage.Url.ToLinkModel();
        }
    }
}