namespace uIntra.Core.Links
{
    public class ActivityCreateLinks : ActivityCreatorLink
    {
        public string Overview { get; }
        public string Create { get; }

        public ActivityCreateLinks(string overview, string create, string creator) : base(creator)
        {
            Overview = overview;
            Create = create;
        }

        public virtual ActivityCreateLinks WithCreate(string value)
        {
            return new ActivityCreateLinks(Overview, value, Creator);
        }
    }
}