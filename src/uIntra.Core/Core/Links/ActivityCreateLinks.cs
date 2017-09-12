namespace uIntra.Core.Links
{
    public class ActivityCreateLinks : ActivityCreatorLink
    {
        public ActivityCreateLinks(string overview, string create, string creator) : base(creator)
        {
            Overview = overview;
            Create = create;
        }

        public string Overview { get; }
        public string Create { get; }
    }
}