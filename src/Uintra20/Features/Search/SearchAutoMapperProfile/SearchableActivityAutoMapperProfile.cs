using AutoMapper;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.Search.Entities;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Search.SearchAutoMapperProfile
{
    public class SearchableActivityAutoMapperProfile : Profile
    {
        public SearchableActivityAutoMapperProfile()
        {
            CreateMap<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Description, o => o.MapFrom(el => el.Description))
                .AfterMap((src, dst) =>
                {
                    dst.Type = src.Type.ToInt();
                });
        }
    }
}