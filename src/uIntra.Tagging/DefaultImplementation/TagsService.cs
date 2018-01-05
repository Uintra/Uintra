using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;

namespace uIntra.Tagging
{
    public class TagsService : ITagsService
    {
        private readonly ISqlRepository<Tag> _tagRepository;
        private readonly ISqlRepository<int, TagActivityRelation> _tagActivityRelationRepository;

        public TagsService(ISqlRepository<Tag> tagRepository, ISqlRepository<int, TagActivityRelation> tagActivityRelationRepository)
        {
            _tagRepository = tagRepository;
            _tagActivityRelationRepository = tagActivityRelationRepository;
        }

        public IEnumerable<Tag> GetAllForActivity(Guid activityId)
        {
            var tagIds = _tagActivityRelationRepository
                .FindAll(el => el.ActivityId == activityId)
                .Select(el => el.TagId)
                .ToList();

            return _tagRepository
                .GetMany(tagIds)
                .OrderBy(tag => tag.Text);
        }

        public IEnumerable<Tag> FindAll(string query)   
        {
            var trimmedQuery = query.Trim();
            return _tagRepository.FindAll(el => el.Text.StartsWith(trimmedQuery));
        }

        public IEnumerable<Tag> GetTagsById(IEnumerable<Guid> tagsIds)
        {
            return _tagRepository.GetMany(tagsIds);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public void SaveRelations(Guid activityId, IEnumerable<TagDTO> tags)
        {
            _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);
            var tagsList = tags.AsList();
            if (tagsList.IsEmpty())
            {
                return;
            }

            var newTags = tagsList.Where(tag => tag.Id == null).ToList();
            var newTagTexts = newTags.Select(t => t.Text).ToList();
            var existedTags = _tagRepository.FindAll(el => newTagTexts.Contains(el.Text)).ToList();

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

            var activityTagIds = tagsList
                .Where(tag => tag.Id.HasValue)
                .Select(tag => tag.Id.Value)
                .Concat(newTagsWithoutIds.Select(el => el.Id))
                .ToList();

            var tagActivityRelations = activityTagIds.Select(el => new TagActivityRelation { ActivityId = activityId, TagId = el });
            _tagActivityRelationRepository.Add(tagActivityRelations);
        }
    }
}
