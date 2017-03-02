using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace uCommunity.Comments.App_Plugins.Comments.Models
{
    public class CommentEditModel
    {
        public string UpdateElementId { get; set; }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }
    }
}