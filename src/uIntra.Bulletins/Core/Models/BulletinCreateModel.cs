using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace uIntra.Bulletins
{
    public class BulletinCreateModel //, IContentWithMediaCreateEditModel
    {
        [Required, AllowHtml, StringLength(2000)]
        public string Description { get; set; }

        //public string Media { get; set; }

        //public int? MediaRootId { get; set; }

        //public string NewMedia { get; set; }

        //public Guid CreatorId { get; set; }
    }
}