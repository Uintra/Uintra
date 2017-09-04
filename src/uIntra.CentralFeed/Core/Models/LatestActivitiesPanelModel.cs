namespace uIntra.CentralFeed
{
    /// <summary>
    /// Bounded with Umbraco's backoffice control.value 
    /// </summary>
    public class LatestActivitiesPanelModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public int ActivityTypeId { get; set; }
        public int ActivityAmount { get; set; }
    }
}
