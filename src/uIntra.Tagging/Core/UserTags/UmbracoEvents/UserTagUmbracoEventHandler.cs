using System.Linq;
using uIntra.Core;
using uIntra.Core.UmbracoEventServices;
using uIntra.Tagging.UserTags.Models;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Tagging.UserTags
{
    public class UserTagUmbracoEventHandler : IUmbracoContentTrashedEventService
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IUserTagRelationService _userTagRelationService;

        public UserTagUmbracoEventHandler(IDocumentTypeAliasProvider documentTypeAliasProvider, IUserTagRelationService userTagRelationService)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _userTagRelationService = userTagRelationService;
        }

        public void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args)
        {
            var trashedUserTagIds = args
                .MoveInfoCollection
                .Where(IsUserTag)
                .Select(arg => arg.Entity.Key)
                .ToList();

            _userTagRelationService.RemoveRelationsForTags(trashedUserTagIds);
        }

        private bool IsUserTag(MoveEventInfo<IContent> arg) => 
            arg.Entity.ContentType.Alias == _documentTypeAliasProvider.GetUserTag();
    }
}
