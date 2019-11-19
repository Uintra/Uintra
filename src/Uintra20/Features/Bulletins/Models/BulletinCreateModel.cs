using System.Collections.Generic;
using Uintra20.Attributes;
using Uintra20.Features.Activity.Models;
using Uintra20.Features.Media;
using Uintra20.Features.User.Models;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        public override string Title { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(NewMedia)), StringLengthWithoutHtml(2000)]
        public string Description { get; set; }

        public int? MediaRootId { get; set; }

        public MemberViewModel Creator { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Description))]
        public string NewMedia { get; set; }

        public string AllowedMediaExtensions { get; set; }

        public IEnumerable<string> Dates { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}