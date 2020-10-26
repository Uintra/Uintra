using Uintra20.Core.Search.Indexers;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.Article.Events
{
    public class ArticlePageEventService: IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService, IUmbracoContentTrashedEventService
    {
        private readonly IContentIndexer _contentIndexer;

        public ArticlePageEventService(
            IContentIndexer contentIndexer)
        {
            _contentIndexer = contentIndexer;
        }

        public void ProcessContentPublished(IContentService sender, PublishEventArgs<IContent> args)
        {
            foreach (var content in args.PublishedEntities)
            {
                _contentIndexer.FillIndex(content.Id);
            }
        }

        public void ProcessContentUnPublished(IContentService sender, PublishEventArgs<IContent> e)
        {
            foreach (var content in e.PublishedEntities)
            {
                _contentIndexer.DeleteFromIndex(content.Id);
            }
        }

        public void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args)
        {
            foreach (var content in args.MoveInfoCollection)
            {
                _contentIndexer.DeleteFromIndex(content.Entity.Id);
            }
        }
    }
}
