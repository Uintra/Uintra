using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Uintra.Comments
{
    public class CommentCreateModel
    {
        public string UpdateElementId { get; set; }

        public Guid? ParentId { get; set; }

        [Required(ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}