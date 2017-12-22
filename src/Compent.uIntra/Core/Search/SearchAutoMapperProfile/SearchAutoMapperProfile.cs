using AutoMapper;
using Compent.uIntra.Core.Search.Entities;
using Compent.uIntra.Core.Search.Models;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Search;
using uIntra.Tagging.UserTags.Models;

namespace Compent.uIntra.Core.Search.SearchAutoMapperProfile
{
    public class SearchAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserTag, SearchableTag>()
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.Title, src => src.MapFrom(s => s.Text))
                .ForMember(dst => dst.Type, src => src.MapFrom(s => UintraSearchableTypeEnum.Tag.ToInt()));

            Mapper.CreateMap<SearchableUintraActivity, UintraSearchResultViewModel>()
                .IncludeBase<SearchableActivity, UintraSearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(dst => dst.Phone, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.PanelContent, src => src.Ignore());

            Mapper.CreateMap<SearchableUser, SearchResultViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(s => s.Id))
                .ForMember(dst => dst.Title, o => o.MapFrom(s => s.FullName))
                .ForMember(dst => dst.Url, o => o.MapFrom(s => s.Url))
                .ForMember(dst => dst.Type, o => o.MapFrom(s => s.Type))
                .ForMember(dst => dst.Description, o => o.Ignore())
                .ForMember(dst => dst.PanelContent, o => o.Ignore())
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            Mapper.CreateMap<SearchableUser, UintraSearchResultViewModel>()
                .IncludeBase<SearchableUser, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.MapFrom(s => s.Phone))
                .ForMember(dst => dst.Email, o => o.MapFrom(s => s.Email))
                .ForMember(dst => dst.Photo, o => o.MapFrom(s => s.Photo));

            Mapper.CreateMap<SearchableActivity, UintraSearchResultViewModel>()
                .IncludeBase<SearchableActivity, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore());

            Mapper.CreateMap<SearchableDocument, UintraSearchResultViewModel>()
                .IncludeBase<SearchableDocument, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, o => o.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore());

            Mapper.CreateMap<SearchableUintraContent, UintraSearchResultViewModel>()
                .IncludeBase<SearchableContent, SearchResultViewModel>()
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore());

            Mapper.CreateMap<SearchableTag, UintraSearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.AdditionalInfo, o => o.Ignore());

            Mapper.CreateMap<SearchableUser, UintraSearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.Title, o => o.MapFrom(s => s.FullName))
                .ForMember(dst => dst.AdditionalInfo, o => o.Ignore());

            Mapper.CreateMap<SearchableActivity, UintraSearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.AdditionalInfo, o => o.Ignore());

            Mapper.CreateMap<SearchableContent, UintraSearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.AdditionalInfo, o => o.Ignore());

            Mapper.CreateMap<SearchableDocument, UintraSearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.AdditionalInfo, o => o.Ignore());

            Mapper.CreateMap<SearchableTag, UintraSearchResultViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(s => s.Id))
                .ForMember(dst => dst.Title, src => src.MapFrom(s => s.Title))
                .ForMember(dst => dst.Type, src => src.MapFrom(s => s.Type))
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.Description, src => src.Ignore())
                .ForMember(dst => dst.PanelContent, src => src.Ignore())
                .ForMember(dst => dst.StartDate, src => src.Ignore())
                .ForMember(dst => dst.EndDate, src => src.Ignore())
                .ForMember(dst => dst.PublishedDate, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => false))
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.Phone, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            Mapper.CreateMap<IntranetActivity, SearchableUintraActivity>()
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.StartDate, src => src.Ignore())
                .ForMember(dst => dst.EndDate, src => src.Ignore())
                .ForMember(dst => dst.PublishedDate, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore())
                .ForMember(dst => dst.Type, src => src.Ignore())
                .ForMember(dst => dst.Description, src => src.MapFrom(el => el.Description))
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .AfterMap((src, dst) => { dst.Type = src.Type.Id; });

            base.Configure();
        }
    }
}