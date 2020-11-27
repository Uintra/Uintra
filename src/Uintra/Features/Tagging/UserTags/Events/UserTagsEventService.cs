using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Indexes;
using Uintra.Core.UmbracoEvents.Services.Contracts;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Providers;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Features.Tagging.UserTags
{
    public class UserTagsEventService : IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService,
        IUmbracoContentTrashedEventService
    {
        private readonly IUserTagProvider _userTagProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IUserTagRelationService _userTagRelationService;
        private readonly IActivityUserTagSearchRepository _activityUserTagSearchRepository;
        private readonly IUserTagProvider _tagProvider;
        private readonly IUserTagIndexer _userTagIndexer;

        public UserTagsEventService(
            IUserTagProvider userTagProvider, 
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IUserTagRelationService userTagRelationService,
            IActivityUserTagSearchRepository activityUserTagSearchRepository,
            IUserTagProvider tagProvider,
            IUserTagIndexer userTagIndexer)
        {
            _userTagProvider = userTagProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _userTagRelationService = userTagRelationService;
            _activityUserTagSearchRepository = activityUserTagSearchRepository;
            _tagProvider = tagProvider;
            _userTagIndexer = userTagIndexer;
        }

        public void ProcessContentPublished(IContentService sender, PublishEventArgs<IContent> args)
        {
            foreach (var pe in args.PublishedEntities)
            {
                if (IsUserTag(pe))
                {
                    var userTag = _userTagProvider.Get(pe.Key);
                    AsyncHelpers.RunSync(() =>_userTagIndexer.Index(userTag));
                    Associate(pe);
                }
            }
        }

        public void ProcessContentUnPublished(IContentService sender, PublishEventArgs<IContent> e)
        {
            foreach (var pe in e.PublishedEntities)
            {
                if (IsUserTag(pe))
                {
                    Disassociate(pe);

                    AsyncHelpers.RunSync(() => _userTagIndexer.Delete(pe.Key));
                }
            }
        }

        public void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args)
        {
            var ids = args
                .MoveInfoCollection
                .Where(mci => IsUserTag(mci.Entity))
                .Select(arg => arg.Entity.Key)
                .ToList();
            
            Disassociate(ids);
            foreach (var id in ids)
            {
                AsyncHelpers.RunSync(() => _userTagIndexer.Delete(id));
            }
            _userTagRelationService.RemoveForTags(ids);

        }
        
        private void Disassociate(IEnumerable<Guid> ids)
        {
            var relations = _userTagRelationService.GetManyRelations(ids);
            if (!relations.Any())
            {
                return;
            }
            var tagNames = _tagProvider.Get(ids).Select(t => t.Text);
            var entityIds = relations.Select(r => r.EntityId).Distinct();
            foreach (var entityId in entityIds)
            {
                _activityUserTagSearchRepository.Remove(entityId,tagNames);
            }
        }
        private void Disassociate(IContent pe)
        {
            var relations = _userTagRelationService.GetRelations(pe.Key);
            if (!relations.Any())
            {
                return;
            }
            var tagName = _tagProvider.Get(pe.Key).Text;
            var entityIds = relations.Select(r => r.EntityId);
            foreach (var entityId in entityIds)
            {
                _activityUserTagSearchRepository.Remove(entityId, tagName.ToEnumerable());
            }
        }
        
        private void Associate(IContent pe)
        {
            var relations = _userTagRelationService.GetRelations(pe.Key);
            if (!relations.Any())
            {
                return;
            }
            var tagName = _tagProvider.Get(pe.Key).Text;
            var entityIds = relations.Select(r => r.EntityId);
            foreach (var entityId in entityIds)
            {
                _activityUserTagSearchRepository.Add(entityId, tagName);
            }
        }

        private bool IsUserTag(IContent content) =>
            content.ContentType.Alias == _documentTypeAliasProvider.GetUserTag();
    }
}