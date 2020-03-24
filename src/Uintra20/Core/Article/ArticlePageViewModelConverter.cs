using Compent.Extensions;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Article
{
    public class ArticlePageViewModelConverter : INodeViewModelConverter<ArticlePageModel, ArticlePageViewModel>
    {
        private readonly IUBaselineRequestContext _context;

        public ArticlePageViewModelConverter(IUBaselineRequestContext context)
        {
            _context = context;
        }

        public void Map(ArticlePageModel node, ArticlePageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!groupId.HasValue) return;

            viewModel.GroupId = groupId;
        }
    }
}