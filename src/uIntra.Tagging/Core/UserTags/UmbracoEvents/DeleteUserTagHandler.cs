using System.Linq;
using Uintra.Core;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Tagging.UserTags
{
    public class DeleteUserTagHandler : IUmbracoContentTrashedEventService
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IUserTagRelationService _userTagRelationService;

        public DeleteUserTagHandler(IDocumentTypeAliasProvider documentTypeAliasProvider, IUserTagRelationService userTagRelationService)
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

            _userTagRelationService.RemoveForTags(trashedUserTagIds);
        }

        private bool IsUserTag(MoveEventInfo<IContent> arg) => 
            arg.Entity.ContentType.Alias == _documentTypeAliasProvider.GetUserTag();
    }
}
