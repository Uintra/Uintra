using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Uintra20.Features.Comments.Models
{
    public class CommentEditModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}