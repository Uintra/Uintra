using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Attributes;
using uIntra.Core.Media;

namespace uIntra.Comments
{
    public class CommentEditModel: IContentWithMediaCreateEditModel
    {
        public string UpdateElementId { get; set; }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*"), AllowHtml]
        public string Text { get; set; }
        
        [Required]
        public int? MediaRootId { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Text))]
        public string NewMedia { get; set; }
    }
}