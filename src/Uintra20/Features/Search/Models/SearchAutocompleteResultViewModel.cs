namespace Uintra20.Features.Search.Models
{
    public class SearchAutocompleteResultViewModel
    {
        public string Title { get; set; }
              
        public string Url { get; set; }

        public SearchBoxAutocompleteItemViewModel Item { get; set; }
    }
}