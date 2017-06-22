using AutoMapper;
using uIntra.Search.Core.Entities;
using uIntra.Search.Core.Models;

namespace uIntra.Search.Core
{
    public class SearchResultAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SearchableBase, SearchAutocompleteResultModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Type, o => o.Ignore());              

            Mapper.CreateMap<SearchableBase, SearchTextResultModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore())
                .ForMember(d => d.PanelContent, o => o.Ignore())
                .ForMember(d => d.StartDate, o => o.Ignore())
                .ForMember(d => d.EndDate, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.Ignore());

            Mapper.CreateMap<SearchableActivity, SearchTextResultModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishedDate))
                .IncludeBase<SearchableBase, SearchTextResultModel>();

            Mapper.CreateMap<SearchableContent, SearchTextResultModel>()
                .ForMember(d => d.PanelContent, o => o.MapFrom(s => s.PanelContent))
                .IncludeBase<SearchableBase, SearchTextResultModel>();
        }
    }
}