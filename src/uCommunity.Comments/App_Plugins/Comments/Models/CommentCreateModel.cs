using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace uCommunity.Comments.App_Plugins.Comments.Models
{
    public class CommentCreateModel
    {
        public string UpdateElementId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid ActivityId { get; set; }

        [Required(ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }
    }
}