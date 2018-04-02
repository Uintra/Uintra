using Uintra.Core;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace Uintra.Tagging.UserTags
{
    public class CreateUserTagHandler : IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService
    {
        private readonly IUserTagProvider _userTagProvider;
        private readonly IUserTagsSearchIndexer _userTagsSearchIndexer;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public CreateUserTagHandler(IUserTagProvider userTagProvider, IUserTagsSearchIndexer userTagsSearchIndexer, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _userTagProvider = userTagProvider;
            _userTagsSearchIndexer = userTagsSearchIndexer;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }
        public void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> args)
        {
            foreach (var pe in args.PublishedEntities)
            {
                if (IsUserTag(pe))
                {
                    var userTag = _userTagProvider.Get(pe.Key);
                    _userTagsSearchIndexer.FillIndex(userTag);
                }
            }
        }

        public void ProcessContentUnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var pe in e.PublishedEntities)
            {
                if (IsUserTag(pe))
                {
                    _userTagsSearchIndexer.Delete(pe.Key);
                }

            }
        }

        private bool IsUserTag(IContent content) =>
            content.ContentType.Alias == _documentTypeAliasProvider.GetUserTag();
    }
}
