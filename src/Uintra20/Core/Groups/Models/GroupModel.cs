using System;

namespace Uintra20.Core.Groups
{
    public class GroupModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid CreatorId { get; set; }

        public int? ImageId { get; set; }

        public bool IsHidden { get; set; }

        public int GroupTypeId { get; set; }
    }


    public static class GroupModelGetters
    {
        public static Guid CreatorId(GroupModel group) => group.CreatorId;

        public static Guid GroupId(GroupModel group) => group.Id;
    }
}