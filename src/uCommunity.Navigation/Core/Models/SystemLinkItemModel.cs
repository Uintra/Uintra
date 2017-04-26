using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Navigation.Core
{
    public class SystemLinkItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public SystemLinkItemModel(string pageTitleNodePropertyAlias, string pageUrlNodePropertyAlias,
            IPublishedContent content)
        {
            Id = content.Id;
            Name = content.GetPropertyValue<string>(pageTitleNodePropertyAlias);
            Url = content.GetPropertyValue<string>(pageUrlNodePropertyAlias);
        }
    }
}
