using uCommunity.Core.Media;

namespace uCommunity.Users.Core
{
    public class ProfileEditModelBase : IContentWithMediaCreateEditModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }

        public int? MediaRootId { get; set; }
        public string NewMedia { get; set; }
    }
}
