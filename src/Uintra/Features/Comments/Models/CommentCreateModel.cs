﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core;

namespace Uintra.Features.Comments.Models
{
    public class CommentCreateModel
    {
        public Guid EntityId { get; set; }

        public IntranetEntityTypeEnum EntityType { get; set; }

        public Guid? ParentId { get; set; }

        [Required(ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}