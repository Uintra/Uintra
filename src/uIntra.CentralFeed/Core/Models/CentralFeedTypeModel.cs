using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public class CentralFeedTypeModel
    {
        public IntranetActivityTypeEnum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string TabUrl { get; set; }
    }
}