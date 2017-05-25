using System.ComponentModel.DataAnnotations;

namespace uIntra.Core.Media
{
    public enum MediaFolderTypeEnum
    {
        [Display(Name = "Miscellaneous Media")]
        Other = 1,

        [Display(Name = "News Content")]
        NewsContent,

        [Display(Name = "Events Content")]
        EventsContent,

        [Display(Name = "Ideas Content")]
        IdeasContent,

        [Display(Name = "Members Content")]
        MembersContent
    }
}
