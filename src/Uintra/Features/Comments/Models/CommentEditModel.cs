using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Uintra.Core;

namespace Uintra.Features.Comments.Models
{
    public class CommentEditModel
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public IntranetEntityTypeEnum EntityType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}