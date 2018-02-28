using AutoMapper;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;

namespace Uintra.Search
{
    public class SearchableActivityAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetActivity, SearchableActivity>()
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

            base.Configure();
        }
    }
}