using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesPanelModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public int ActivityTypeId { get; set; }
        public int ActivityAmount { get; set; }
    }
}
