﻿using AutoMapper;
using Uintra.Core.Search.Entities;
using Uintra.Features.Search.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Search.AutoMapperProfile
{
    public class SearchResultAutoMapperProfile : Profile
    {
        public SearchResultAutoMapperProfile()
        {
            CreateMap<SearchableBase, SearchAutocompleteResultViewModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Item, o => o.Ignore());

            CreateMap<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore())
                .ForMember(d => d.Panels, o => o.Ignore())
                .ForMember(d => d.StartDate, o => o.Ignore())
                .ForMember(d => d.EndDate, o => o.Ignore())
                .ForMember(d => d.IsPinned, o => o.Ignore())
                .ForMember(d => d.IsPinActual, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.Ignore());

            CreateMap<SearchableActivity, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate.ToDateTimeFormat()))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate.ToDateTimeFormat()))
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishedDate.ToDateTimeFormat()))
                .ForMember(d => d.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(d => d.IsPinActual, o => o.MapFrom(s => s.IsPinActual));

            CreateMap<SearchableContent, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Panels, o => o.MapFrom(s => s.Panels));

            CreateMap<SearchablePanel, SearchPanelResultViewModel>()
                .ForMember(d => d.Content, o => o.MapFrom((s => s.Content)))
                .ForMember(d => d.Title, o => o.MapFrom((s => s.Title)));
            
            CreateMap<SearchableDocument, SearchResultViewModel>()
                .IncludeBase<SearchableBase, SearchResultViewModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Attachment.Content));
        }
    }
}