using System;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.News.Converters.Models;

namespace Uintra20.Features.News.Converters
{
    public class NewsDetailsPageViewModelConverter : INodeViewModelConverter<NewsDetailsPageModel, NewsDetailsPageViewModel>
    {
        public void Map(NewsDetailsPageModel node, NewsDetailsPageViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        private IntranetActivityDetailsViewModel GetDetails(Guid activityId)
        {

        }
    }
}