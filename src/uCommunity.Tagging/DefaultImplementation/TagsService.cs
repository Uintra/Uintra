using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Tagging
{
    public class TagsService : ITagsService
    {
        private readonly ISqlRepository<Tag> _tagRepository;
        private readonly ISqlRepository<TagActivityRelation> _tagActivityRelationRepository;

        public TagsService(ISqlRepository<Tag> tagRepository, ISqlRepository<TagActivityRelation> tagActivityRelationRepository)
        {
            _tagRepository = tagRepository;
            _tagActivityRelationRepository = tagActivityRelationRepository;
        }

        public void FillTags(IHaveTags activity)
        {
            activity.Tags = GetMany(activity.Id);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public Tag Add(Guid activityId, string text, Guid? tagId = null)
        {
            if (!tagId.HasValue)
            {
                var tag = new Tag { Id = Guid.NewGuid(), Text = text };
                _tagRepository.Add(tag);
                tagId = tag.Id;
            }

            var tagActivityRelation = new TagActivityRelation
            {
                TagId = tagId.Value,
                ActivityId = activityId
            };

            _tagActivityRelationRepository.Add(tagActivityRelation);

            return _tagRepository.Get(tagId.Value);
        }

        public void Remove(Guid tagId, Guid activityId)
        {
            _tagActivityRelationRepository.Delete(el => el.TagId == tagId && el.ActivityId == activityId);
        }

        private IEnumerable<Tag> GetMany(Guid activityId)
        {
            var tagIds = _tagActivityRelationRepository
                .FindAll(el => el.ActivityId == activityId)
                .Select(el => el.TagId)
                .ToList();

            return _tagRepository.FindAll(el => tagIds.Contains(el.Id));
        }
    }
}
