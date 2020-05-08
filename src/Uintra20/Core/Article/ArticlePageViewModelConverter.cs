using Compent.Extensions;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Features.Breadcrumbs.Services.Contracts;
using Uintra20.Features.Navigation;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Article
{
    public class ArticlePageViewModelConverter : INodeViewModelConverter<ArticlePageModel, ArticlePageViewModel>
    {
        private readonly IUBaselineRequestContext _context;
        private readonly IBreadcrumbService _breadcrumbService;


        public ArticlePageViewModelConverter(
            IUBaselineRequestContext context,
            IBreadcrumbService breadcrumbService)
        {
            _context = context;
            _breadcrumbService = breadcrumbService;
        }

        public void Map(ArticlePageModel node, ArticlePageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();
            viewModel.Breadcrumbs = _breadcrumbService.GetBreadcrumbsItems();   
            viewModel.GroupId = groupId;
        }
    }
}