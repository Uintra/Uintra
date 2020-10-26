using System.Collections.Generic;
using Uintra.Core.LinksPicker;
using Uintra.Core.UbaselineModels;

namespace Uintra.Features.Navigation.Models
{
    public class SharedLinkApiViewModel
    {
        public string LinksGroupTitle { get; set; }
        public IEnumerable<UintraLinksPickerViewModel> Links { get; set; }
    }
}