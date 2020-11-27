using System.Linq;
using Uintra.Core.Search.Indexers;
using Uintra.Core.UmbracoEvents.Services.Contracts;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.Article.Events
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
                // TODO: Search. Add by many, to avoid many operations
                _contentIndexer.Index(content.Id);
            }
        }

        public void ProcessContentUnPublished(IContentService sender, PublishEventArgs<IContent> e)
        {
            var ids = e.PublishedEntities.Select(c => c.Id.ToString());
            _contentIndexer.Delete(ids);
        }

        public void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args)
        {
            var ids = args.MoveInfoCollection.Select(c => c.Entity.Id.ToString());
            _contentIndexer.Delete(ids);
        }
    }
}
