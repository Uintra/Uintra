using System;
using AutoMapper;
using Compent.Uintra.Core.Bulletins;
using Compent.Uintra.Core.Events;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Groups;
using Uintra.Search;
using Uintra.Tagging.UserTags.Models;

namespace Compent.Uintra.Core.Search.SearchAutoMapperProfile
{
    public class SearchAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserTag, SearchableTag>()
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.Title, src => src.MapFrom(s => s.Text))
                .ForMember(dst => dst.Type, src => src.MapFrom(s => UintraSearchableTypeEnum.Tag.ToInt()));

            Mapper.CreateMap<IGroupMember, SearchableMember>()
                .ForMember(dst => dst.FullName, o => o.MapFrom(src => src.DisplayedName))
                .ForMember(dst => dst.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dst => dst.Phone, o => o.MapFrom(src => src.Phone))
                .ForMember(dst => dst.Photo, o => o.MapFrom(src => src.Photo.IfNone(() => string.Empty)))
                .ForMember(dst => dst.Department, o => o.MapFrom(src => src.Department))
                .ForMember(dst => dst.Type, o => o.MapFrom(src => (int)UintraSearchableTypeEnum.User))
                .ForMember(dst => dst.Inactive, o => o.MapFrom(src => src.Inactive))
                .ForMember(dst => dst.Groups, o => o.Ignore())
				.ForMember(dst => dst.UserTagNames, o => o.Ignore())
                .ForMember(dst => dst.TagsHighlighted, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Title, o => o.Ignore());

            Mapper.CreateMap<SearchableUintraActivity, UintraSearchResultViewModel>()
                .IncludeBase<SearchableActivity, UintraSearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.PanelContent, src => src.Ignore());

            Mapper.CreateMap<SearchableMember, SearchResultViewModel>()
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

            Mapper.CreateMap<SearchableMember, UintraSearchResultViewModel>()
                .IncludeBase<SearchableMember, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.MapFrom(s => s.Email))
                .ForMember(dst => dst.Photo, o => o.MapFrom(s => s.Photo));

            Mapper.CreateMap<SearchableActivity, UintraSearchResultViewModel>()
                .IncludeBase<SearchableActivity, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore());

            Mapper.CreateMap<SearchableDocument, UintraSearchResultViewModel>()
                .IncludeBase<SearchableDocument, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, o => o.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore());

            Mapper.CreateMap<SearchableUintraContent, UintraSearchResultViewModel>()
                .IncludeBase<SearchableContent, SearchResultViewModel>()
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore());

            Mapper.CreateMap<SearchableMember, SearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.Title, o => o.MapFrom(s => s.FullName));

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
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            Mapper.CreateMap<IntranetActivity, SearchableUintraActivity>()
                .ForMember(dst => dst.Description, src => src.MapFrom(el => el.Description))
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.StartDate, src => src.Ignore())
                .ForMember(dst => dst.EndDate, src => src.Ignore())
                .ForMember(dst => dst.PublishedDate, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore())
                .ForMember(dst => dst.Type, src => src.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .AfterMap((src, dst) => { dst.Type = src.Type.ToInt(); });

            Mapper.CreateMap<Bulletin, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.Title = src.Description?.StripHtml().TrimByWordEnd(50);
                });

            Mapper.CreateMap<Bulletin, SearchableUintraActivity>()
                .IncludeBase<Bulletin, SearchableActivity>()
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore());

            Mapper.CreateMap<News.Entities.News, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore());

            Mapper.CreateMap<News.Entities.News, SearchableUintraActivity>()
                .IncludeBase<News.Entities.News, SearchableActivity>()
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore());

            Mapper.CreateMap<Event, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(dst => dst.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore());

            Mapper.CreateMap<Event, SearchableUintraActivity>()
                .IncludeBase<Event, SearchableActivity>()
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore());

            base.Configure();
        }
    }
}