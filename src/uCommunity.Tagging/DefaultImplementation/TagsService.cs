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
            activity.Tags = GetMany(activity.Id).Select(el => el.Text).OrderBy(el => el);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public void SaveTags(Guid activityId, IEnumerable<string> tags)
        {
            if (tags == null)
            {
                _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);
                return;
            }

            var trimmedTags = tags.Select(el => el.Trim()).ToList();

            var activityTags = new List<Tag>();
            var allTags = GetAll().ToList();

            foreach (var tagText in trimmedTags)
            {
                var tag = allTags.Find(el => el.Text.Equals(tagText));
                if (tag == null)
                {
                    tag = new Tag { Id = Guid.NewGuid(), Text = tagText };
                    _tagRepository.Add(tag);
                }

                activityTags.Add(tag);
            }

            _tagActivityRelationRepository.Delete(el => el.ActivityId == activityId);

            var tagActivityRelations = activityTags.Select(el => new TagActivityRelation { ActivityId = activityId, TagId = el.Id });
            _tagActivityRelationRepository.Add(tagActivityRelations);
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
