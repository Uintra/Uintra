using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Persistence.Sql;

namespace uIntra.Tagging
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

        public IEnumerable<Tag> GetMany(Guid activityId)
        {
            var tagIds = _tagActivityRelationRepository
                .FindAll(el => el.ActivityId == activityId)
                .Select(el => el.TagId)
                .ToList();

            return _tagRepository
                .GetMany(tagIds.Cast<object>())
                .OrderBy(tag => tag.Text);
        }

        public IEnumerable<Tag> FindAll(string query)
        {
            var trimmedQuery = query.Trim();
            return _tagRepository.FindAll(el => el.Text.StartsWith(trimmedQuery, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public void Save(Guid activityId, IEnumerable<TagDTO> tags)
        {
            _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);
            if (tags.IsEmpty())
            {
                return;
            }

            var newTags = tags.Where(tag => tag.Id == null).ToList();
            var existedTags = _tagRepository.FindAll(el => newTags.Select(t => t.Text).Contains(el.Text)).ToList();

            newTags = newTags.GroupJoin(
                existedTags,
                t => t.Text,
                db => db.Text,
                (t, dbTags) =>
                {
                    var tag = dbTags.FirstOrDefault();
                    t.Id = tag?.Id;
                    return t;
                }).ToList();

            var newTagsWithoutIds = newTags
                .Where(tag => tag.Id == null)
                .Select(tag => new Tag { Id = Guid.NewGuid(), Text = tag.Text })
                .ToList();

            _tagRepository.Add(newTagsWithoutIds);

            var activityTagIds = tags
                .Where(tag => tag.Id.HasValue)
                .Select(tag => tag.Id.Value)
                .Concat(newTagsWithoutIds.Select(el => el.Id))
                .ToList();

            var tagActivityRelations = activityTagIds.Select(el => new TagActivityRelation { ActivityId = activityId, TagId = el });
            _tagActivityRelationRepository.Add(tagActivityRelations);
        }
    }
}
