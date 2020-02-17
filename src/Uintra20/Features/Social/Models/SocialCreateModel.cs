using System.Collections.Generic;
using Uintra20.Attributes;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Media;

namespace Uintra20.Features.Social.Models
{
    public class SocialCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        [RequiredVirtual(IsRequired = false)]
        public override string Title { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(NewMedia)), StringLengthWithoutHtml(2000)]
        public string Description { get; set; }

        public string AllowedMediaExtensions { get; set; }

        public IEnumerable<string> Dates { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Description))]
        public string NewMedia { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}