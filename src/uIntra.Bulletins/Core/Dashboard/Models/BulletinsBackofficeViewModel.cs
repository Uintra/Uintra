﻿using System;

namespace uIntra.Bulletins
{
    public class BulletinsBackofficeViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public Guid CreatorId { get; set; }
        public string PublishDate { get; set; }
        public string ModifyDate { get; set; }
        public string CreatedDate { get; set; }
    }
}