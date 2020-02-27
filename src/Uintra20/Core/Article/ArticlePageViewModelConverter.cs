using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Article
{
    public class ArticlePageViewModelConverter : INodeViewModelConverter<ArticlePageModel, ArticlePageViewModel>
    {
        public void Map(ArticlePageModel node, ArticlePageViewModel viewModel)
        {
            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            if (!Guid.TryParse(groupIdStr, out Guid groupId))
                return;

            viewModel.GroupId = groupId;
        }
    }
}