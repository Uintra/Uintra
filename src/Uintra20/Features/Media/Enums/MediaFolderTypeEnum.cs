using System.ComponentModel.DataAnnotations;

namespace Uintra20.Features.Media.Enums
{
    public enum MediaFolderTypeEnum
    {
        [Display(Name = "Miscellaneous Media")]
        Other = 1,

        [Display(Name = "News Content")]
        NewsContent,

        [Display(Name = "Events Content")]
        EventsContent,

        [Display(Name = "Members Content")]
        MembersContent,

        [Display(Name = "Socials Content")]
        SocialsContent,

        [Display(Name = "Comments Content")]
        CommentsContent,

        [Display(Name = "Groups Content")]
        GroupsContent,
    }
}