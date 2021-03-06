﻿using AutoMapper;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Search.Entities;
using Uintra.Features.Events.Entities;
using Uintra.Features.Groups;
using Uintra.Features.Search.Models;
using Uintra.Features.Tagging.UserTags.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Search.AutoMapperProfile
{
    public class SearchAutoMapperProfile : Profile
    {
        public SearchAutoMapperProfile()
        {
            CreateMap<UserTag, SearchableTag>()
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.Culture, src => src.Ignore())
                .ForMember(dst => dst.Title, src => src.MapFrom(s => s.Text))
                .ForMember(dst => dst.Type, src => src.MapFrom(s => UintraSearchableTypeEnum.Tag.ToInt()));

            CreateMap<IGroupMember, SearchableMember>()
                .ForMember(dst => dst.FullName, o => o.MapFrom(src => src.DisplayedName))
                .ForMember(dst => dst.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dst => dst.Phone, o => o.MapFrom(src => src.Phone))
                .ForMember(dst => dst.Photo, o => o.MapFrom(src => src.Photo))
                .ForMember(dst => dst.Department, o => o.MapFrom(src => src.Department))
                .ForMember(dst => dst.Type, o => o.MapFrom(src => (int) UintraSearchableTypeEnum.Member))
                .ForMember(dst => dst.Inactive, o => o.MapFrom(src => src.Inactive))
                .ForMember(dst => dst.Groups, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, o => o.Ignore())
                .ForMember(dst => dst.TagsHighlighted, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Title, o => o.Ignore())
                .ForMember(dst => dst.Culture, o => o.Ignore());

            CreateMap<SearchableActivity, UintraSearchResultViewModel>()
                .IncludeBase<SearchableActivity, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Panels, src => src.Ignore());

            CreateMap<SearchableMember, SearchResultViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(s => s.Id))
                .ForMember(dst => dst.Title, o => o.MapFrom(s => s.FullName))
                .ForMember(dst => dst.Url, o => o.MapFrom(s => s.Url))
                .ForMember(dst => dst.Type, o => o.MapFrom(s => s.Type))
                .ForMember(dst => dst.Description, o => o.MapFrom(s => s.Email))
                .ForMember(dst => dst.Panels, o => o.Ignore())
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            CreateMap<SearchableMember, UintraSearchResultViewModel>()
                .IncludeBase<SearchableMember, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => s.TagsHighlighted))
                .ForMember(dst => dst.UserTagNames, src => src.MapFrom(s => s.UserTagNames))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.MapFrom(s => s.Email))
                .ForMember(dst => dst.Photo, o => o.MapFrom(s => s.Photo));


            CreateMap<SearchableDocument, UintraSearchResultViewModel>()
                .IncludeBase<SearchableDocument, SearchResultViewModel>()
                .ForMember(dst => dst.TagsHighlighted, o => o.MapFrom(s => false))
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore());

            //CreateMap<SearchableUintraContent, UintraSearchResultViewModel>()
            //    .IncludeBase<SearchableContent, SearchResultViewModel>()
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.Email, o => o.Ignore())
            //    .ForMember(dst => dst.Photo, o => o.Ignore());

            CreateMap<SearchableMember, SearchAutocompleteResultViewModel>()
                .IncludeBase<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(dst => dst.Title, o => o.MapFrom(s => s.FullName));

            CreateMap<SearchableTag, UintraSearchResultViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(s => s.Id))
                .ForMember(dst => dst.Title, src => src.MapFrom(s => s.Title))
                .ForMember(dst => dst.Type, src => src.MapFrom(s => s.Type))
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.Description, src => src.Ignore())
                .ForMember(dst => dst.Panels, src => src.Ignore())
                .ForMember(dst => dst.StartDate, src => src.Ignore())
                .ForMember(dst => dst.EndDate, src => src.Ignore())
                .ForMember(dst => dst.PublishedDate, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.MapFrom(s => false))
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.Email, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            CreateMap<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.Description, src => src.MapFrom(el => el.Description))
                .ForMember(dst => dst.Url, src => src.Ignore())
                .ForMember(dst => dst.StartDate, src => src.Ignore())
                .ForMember(dst => dst.EndDate, src => src.Ignore())
                .ForMember(dst => dst.PublishedDate, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore())
                .ForMember(dst => dst.Type, src => src.Ignore())
                .ForMember(dst => dst.Culture, src => src.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .AfterMap((src, dst) => { dst.Type = src.Type.ToInt(); });

            CreateMap<Social.Entities.Social, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore())
                .AfterMap((src, dst) => { dst.Title = src.Description?.StripHtml().TrimByWordEnd(50); });
            
            CreateMap<News.Entities.News, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore());
            
            CreateMap<Event, SearchableActivity>()
                .IncludeBase<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(dst => dst.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.UserTagNames, src => src.Ignore())
                .ForMember(dst => dst.TagsHighlighted, src => src.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore());
        }
    }
}