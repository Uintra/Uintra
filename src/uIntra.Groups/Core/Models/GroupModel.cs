using System;

namespace uIntra.Groups
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
}