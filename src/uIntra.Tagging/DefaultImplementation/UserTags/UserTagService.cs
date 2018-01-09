using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging.UserTags
{
    public class UserTagService : IUserTagService
    {
        private readonly IUserTagRelationService _relationService;
        private readonly IUserTagProvider _tagProvider;

        public UserTagService(IUserTagRelationService relationService, IUserTagProvider tagProvider)
        {
            _relationService = relationService;
            _tagProvider = tagProvider;
        }

        public virtual IEnumerable<UserTag> Get(Guid entityId)
        {
            var tagIds = _relationService.GetForEntity(entityId);
            var tags = _tagProvider.Get(tagIds);
            return tags;
        }

        public virtual void Replace(Guid entityId, IEnumerable<Guid> tagIds)
        {
            var tagIdsList = tagIds.AsList();

            var existedTagIds = _relationService.GetForEntity(entityId).ToList();
            var tagsToDelete = existedTagIds.Except(tagIdsList);
            var tagsToAdd = tagIdsList.Except(existedTagIds);

            _relationService.Remove(entityId, tagsToDelete);
            _relationService.Add(entityId, tagsToAdd);
        }

        public virtual void DeleteAllFor(Guid entityId)
        {
            var existedTagIds = _relationService.GetForEntity(entityId).ToList();
            _relationService.Remove(entityId, existedTagIds);
        }
    }
}