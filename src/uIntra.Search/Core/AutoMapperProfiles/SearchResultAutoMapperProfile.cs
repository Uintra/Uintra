using AutoMapper;

namespace Uintra.Search
{
    public class SearchResultAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Html, o => o.Ignore());

            Mapper.CreateMap<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore())
                .ForMember(d => d.PanelContent, o => o.Ignore())
                .ForMember(d => d.StartDate, o => o.Ignore())
                .ForMember(d => d.EndDate, o => o.Ignore())
                .ForMember(d => d.IsPinned, o => o.Ignore())
                .ForMember(d => d.IsPinActual, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.Ignore());

            Mapper.CreateMap<SearchableActivity, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishedDate))
                .ForMember(d => d.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(d => d.IsPinActual, o => o.MapFrom(s => s.IsPinActual));

            Mapper.CreateMap<SearchableContent, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.PanelContent, o => o.MapFrom(s => s.PanelContent));

            Mapper.CreateMap<SearchableDocument, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Attachment.Content));
        }
    }
}