using uIntra.Core.Media;

namespace uIntra.Users
{
    public class ProfileEditModel : IContentWithMediaCreateEditModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }

        public int? MediaRootId { get; set; }
        public string NewMedia { get; set; }
    }
}
