using System.Collections.Generic;
using System.Linq;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Models
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