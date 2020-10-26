using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using Uintra20.Core.Search.Indexers;
using Uintra20.Core.Search.Indexes;
using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Features.Tagging.UserTags
{
    public class UserTagsEventService : IUmbracoContentPublishedEventService, IUmbracoContentUnPublishedEventService,
        IUmbracoContentTrashedEventService
    {
        private readonly IUserTagProvider _userTagProvider;
        private readonly IUserTagsSearchIndexer _userTagsSearchIndexer;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IUserTagRelationService _userTagRelationService;
        private readonly IActivityUserTagIndex _activityUserTagIndex;
        private readonly IUserTagProvider _tagProvider;

        public UserTagsEventService(
            IUserTagProvider userTagProvider, 
            IUserTagsSearchIndexer userTagsSearchIndexer,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IUserTagRelationService userTagRelationService,
            IActivityUserTagIndex activityUserTagIndex,
            IUserTagProvider tagProvider)
        {
            _userTagProvider = userTagProvider;
            _userTagsSearchIndexer = userTagsSearchIndexer;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _userTagRelationService = userTagRelationService;
            _activityUserTagIndex = activityUserTagIndex;
            _tagProvider = tagProvider;
        }

        public void ProcessContentPublished(IContentService sender, PublishEventArgs<IContent> args)
        {
            foreach (var pe in args.PublishedEntities)
            {
                if (IsUserTag(pe))
                {
                    var userTag = _userTagProvider.Get(pe.Key);
                    _userTagsSearchIndexer.FillIndex(userTag);
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
                    _userTagsSearchIndexer.Delete(pe.Key);
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
                _userTagsSearchIndexer.Delete(id);    
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
                _activityUserTagIndex.Remove(entityId,tagNames);
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
                _activityUserTagIndex.Remove(entityId, tagName.ToEnumerable());
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
                _activityUserTagIndex.Add(entityId, tagName);
            }
        }

        private bool IsUserTag(IContent content) =>
            content.ContentType.Alias == _documentTypeAliasProvider.GetUserTag();
    }
}