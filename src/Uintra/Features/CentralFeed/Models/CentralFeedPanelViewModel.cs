using System;
using UBaseline.Shared.Node;
using Uintra.Core.Feed.Models;

namespace Uintra.Features.CentralFeed.Models
{
	public class CentralFeedPanelViewModel : FeedOverviewModel, INodeViewModel
	{
        public int Id { get; set;}
		public string ContentTypeAlias { get; set; }
		public string Name { get; set; }
		public bool AddToSitemap { get; set; }
		public string Url { get; set; }
        public NodeType NodeType { get; set; }
        public int ItemsPerRequest { get; set; }
        public Guid? GroupId { get; set; }
    }
}