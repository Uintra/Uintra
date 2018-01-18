using System.Collections.Generic;
using System.Linq;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Models
{
    public class UintraSearchAutocompleteResultViewModel : SearchAutocompleteResultViewModel
    {
        public IEnumerable<SearchInfoListItemModel> AdditionalInfo { get; set; }

        public UintraSearchAutocompleteResultViewModel()
        {
            AdditionalInfo = Enumerable.Empty<SearchInfoListItemModel>();
        }
    }
}