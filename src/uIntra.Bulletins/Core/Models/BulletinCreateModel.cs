using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Attributes;
using uIntra.Core.Media;
using uIntra.Core.User;

namespace uIntra.Bulletins
{
    public class BulletinCreateModel : IntranetActivityCreateModelBase, IContentWithMediaCreateEditModel
    {
        [RequiredVirtual(IsRequired = false)]
        public override string Title { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(NewMedia)), AllowHtml, StringLength(2000)]
        public string Description { get; set; }

        public int? MediaRootId { get; set; }

        public IIntranetUser Creator { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Description))]
        public string NewMedia { get; set; }

        public string AllowedMediaExtensions { get; set; }

        public IEnumerable<string> Dates { get; set; }
    }
}