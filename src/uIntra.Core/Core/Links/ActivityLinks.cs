namespace uIntra.Core.Links
{
    public class ActivityLinks : ActivityCreateLinks
    {

        public string Details { get; }
        public string Edit { get; }

        public ActivityLinks(string overview, string create, string details, string edit, string creator, string detailsNoId) : base(overview, create, creator, detailsNoId)
        {
            Details = details;
            Edit = edit;
        }

        public new ActivityLinks WithCreate(string value) => new ActivityLinks(Overview, value, Details, Edit, Creator, DetailsNoId);

        public ActivityLinks WithDetails(string value) => new ActivityLinks(Overview, Create, value, Edit, Creator, DetailsNoId);

        public new ActivityLinks WithDetailsNoId(string value) => new ActivityLinks(Overview, Create, Details, Edit, Creator, value);

        public ActivityLinks WithEdit(string value) => new ActivityLinks(Overview, Create, Details, value, Creator, DetailsNoId);
    }
}