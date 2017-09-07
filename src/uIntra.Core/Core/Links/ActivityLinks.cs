namespace uIntra.Core.Links
{
    public class ActivityLinks
    {
        public ActivityLinks(string overview, string create, string details, string edit, string profile)
        {
            Overview = overview;
            Create = create;
            Details = details;
            Edit = edit;
            Profile = profile;
        }

        public string Overview { get; }
        public string Create { get; }
        public string Details { get; }
        public string Edit { get; }
        public string Profile { get; }
    }
}