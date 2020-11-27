using AutoMapper;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Search.Entities;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Search.AutoMapperProfile
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
                .ForMember(dst => dst.Culture, o => o.Ignore())
                .ForMember(dst => dst.Description, o => o.MapFrom(el => el.Description))
                .AfterMap((src, dst) =>
                {
                    dst.Type = src.Type.ToInt();
                });
        }
    }
}