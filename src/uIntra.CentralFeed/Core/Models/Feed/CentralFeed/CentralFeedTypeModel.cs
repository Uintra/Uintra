using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class CentralFeedTypeModel
    {
        public IIntranetType Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string TabUrl { get; set; }
    }
}