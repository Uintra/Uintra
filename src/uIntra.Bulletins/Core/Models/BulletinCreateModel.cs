using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uIntra.Core.Attributes;
using uIntra.Core.Media;

namespace uIntra.Bulletins
{
    public class BulletinCreateModel : IContentWithMediaCreateEditModel
    {
        [RequiredIfEmpty(OtherProperty = nameof(NewMedia)), AllowHtml, StringLength(2000)]
        public string Description { get; set; }

        public int? MediaRootId { get; set; }

        [RequiredIfEmpty(OtherProperty = nameof(Description))]
        public string NewMedia { get; set; }
    }
}