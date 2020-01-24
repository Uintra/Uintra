using System.Collections.Generic;
using Uintra20.Core.UbaselineModels;

namespace Uintra20.Features.Navigation.Models
{
    public class SharedLinkApiViewModel
    {
        public string LinksGroupTitle { get; set; }
        public IEnumerable<UintraLinksPickerViewModel> Links { get; set; }
        public int Sort { get; set; }
    }
}