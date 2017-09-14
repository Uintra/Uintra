namespace uIntra.Core.Links
{
    public class ActivityCreateLinks : ActivityCreatorLink
    {
        public string Overview { get; }
        public string Create { get; }
        public string DetailsNoId { get; }

        public ActivityCreateLinks(string overview, string create, string creator, string detailsNoId) : base(creator)
        {
            Overview = overview;
            Create = create;
            DetailsNoId = detailsNoId;
        }

        public virtual ActivityCreateLinks WithCreate(string value) => new ActivityCreateLinks(Overview, value, Creator, DetailsNoId);

        public virtual ActivityCreateLinks WithDetailsNoId(string value) => new ActivityCreateLinks(Overview, Create, Creator, value);
    }
}