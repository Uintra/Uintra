using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Attributes;
using uIntra.Core.Media;

namespace uIntra.Comments
{
    public class CommentCreateModel: IContentWithMediaCreateEditModel
    {
        public string UpdateElementId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid ActivityId { get; set; }

        [Required(ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }
        
        [Required]
        public int? MediaRootId { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Text))]
        public string NewMedia { get; set; }
    }
}