namespace uIntra.Core.Links
{
    public class ActivityLinks : ActivityCreateLinks
    {

        public string Details { get; }
        public string Edit { get; }

        public ActivityLinks(string overview, string create, string details, string edit, string creator) : base(overview, create, creator)
        {
            Details = details;
            Edit = edit;
        }
    }
}