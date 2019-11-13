using System.Collections.Generic;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Media;
using Uintra20.Core.User;

namespace Uintra20.Core.Bulletins
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