using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesPanelModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IIntranetType TypeOfActivities { get; set; }
        public int NumberOfActivites { get; set; }
    }
}
