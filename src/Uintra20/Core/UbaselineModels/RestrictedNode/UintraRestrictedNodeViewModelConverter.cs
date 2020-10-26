using System.Net;
using UBaseline.Core.Node;
using UBaseline.Shared.Node;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.UbaselineModels.RestrictedNode
{
    public abstract class UintraRestrictedNodeViewModelConverter<TNodeModel, TNodeViewModel> : INodeViewModelConverter<TNodeModel, TNodeViewModel> 
        where TNodeModel : INodeModel 
        where TNodeViewModel : UintraRestrictedNodeViewModel
    {
        protected readonly IErrorLinksService ErrorLinksService;

        protected UintraRestrictedNodeViewModelConverter(IErrorLinksService errorLinksService)
        {
            ErrorLinksService = errorLinksService;
        }

        //Ubaseline method
        public void Map(TNodeModel node, TNodeViewModel viewModel)
        {
            var result = MapViewModel(node, viewModel);

            viewModel.StatusCode = result.StatusCode;
            viewModel.ErrorLink = result.Link;
        }

        public abstract ConverterResponseModel MapViewModel(TNodeModel node, TNodeViewModel viewModel);

        protected virtual ConverterResponseModel NotFoundResult()
        {
            return new ConverterResponseModel
            {
                StatusCode = HttpStatusCode.NotFound,
                Link = ErrorLinksService.GetNotFoundPageLink()
            };
        }

        protected virtual ConverterResponseModel ForbiddenResult()
        {
            return new ConverterResponseModel
            {
                StatusCode = HttpStatusCode.Forbidden, 
                Link = ErrorLinksService.GetForbiddenPageLink()
            };
        }

        protected virtual ConverterResponseModel OkResult()
        {
            return new ConverterResponseModel
            {
                StatusCode = HttpStatusCode.OK,
                Link = null
            };
        }

        protected virtual ConverterResponseModel CustomResult(HttpStatusCode statusCode, UintraLinkModel link)
        {
            return new ConverterResponseModel
            {
                StatusCode = statusCode,
                Link = link
            };
        }
    }
}