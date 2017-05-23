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
            activity.Tags = GetMany(activity.Id).OrderBy(tag => tag.Text);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public void Save(Guid activityId, IEnumerable<TagDTO> tags)
        {
            if (!tags.Any())
            {
                _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);
                return;
            }

            var newTags = tags
                .Where(tag => tag.Id == null)
                .Select(tag => new Tag { Id = Guid.NewGuid(), Text = tag.Text })
                .ToList();

            _tagRepository.Add(newTags);

            _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);

            var activityTagIds = newTags
                .Select(tag => tag.Id)
                .Concat(tags.Where(t => t.Id.HasValue).Select(t => t.Id.Value));

            var tagActivityRelations = activityTagIds.Select(el => new TagActivityRelation { ActivityId = activityId, TagId = el });
            _tagActivityRelationRepository.Add(tagActivityRelations);
        }

        public IEnumerable<Tag> FindAll(string query)
        {
            var trimmedQuery = query.Trim();
            return _tagRepository.FindAll(el => el.Text.StartsWith(trimmedQuery, StringComparison.OrdinalIgnoreCase));
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
